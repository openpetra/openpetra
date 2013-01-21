//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2013 by OM International
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
using System.Globalization;
using System.Threading;

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Remoting.Server;

namespace Ict.Petra.Server.App.Core.Processing
{
    /// <summary>
    /// Provides means to run certain processing routines at timed intervals.
    /// </summary>
    public static class TTimedProcessing
    {
        /// <summary>Resourcestring used for logging purposes.</summary>
        public const string StrAutomaticProcessing = "Automatic Processing";

        private static int MINUTES_DELAY_BETWEEN_INDIV_PROCESSES = 5;
        delegate void TRemindersProcessor (System.Object AState);
        delegate void TAutomatedIntranetExportProcessor (System.Object AState);

        private static TRemindersProcessor FRemindersProcessor;
        private static TAutomatedIntranetExportProcessor FIntranetExportProcessor;
        private static System.Threading.Timer FRemindersTimer;
        private static System.Threading.Timer FIntranetExportTimer;

        private static DateTime FDailyStartTime24Hrs;
        private static bool FProcessPartnerRemindersEnabled = false;
        private static bool FProcessAutomatedIntranetExportEnabled = false;

        /// <summary>Daily start time of Processing in 24 Hrs Format (with leading zeroes for hours and minutes between 0-9) (this is taken by reading a value from the Server Config file).</summary>
        public static string DailyStartTime24Hrs
        {
            get
            {
                return FDailyStartTime24Hrs.ToLongTimeString();
            }

            set
            {
                try
                {
                    FDailyStartTime24Hrs = DateTime.ParseExact(value, "HH:mm", null, DateTimeStyles.NoCurrentDateDefault);
                }
                catch (System.FormatException)
                {
                    throw new ApplicationException(
                        "Server Configuration File error: The value supplied for 'Server.Processing.DailyStartTime24Hrs' (" + value +
                        ") isn't a valid time in 24 hours format (leading zeroes are required for hours and minutes between 0-9)");
                }
            }
        }

        /// <summary>Whether the Partner Reminders processing is enabled (this is decided by reading a value from the Server Config file).</summary>
        public static bool ProcessPartnerRemindersEnabled
        {
            get
            {
                return FProcessPartnerRemindersEnabled;
            }

            set
            {
                FProcessPartnerRemindersEnabled = value;
            }
        }

        /// <summary>Whether the Automated Intranet (Caleb) Export is enabled (this is decided by reading a value from the Server Config file).</summary>
        public static bool ProcessAutomatedIntranetExportEnabled
        {
            get
            {
                return FProcessAutomatedIntranetExportEnabled;
            }

            set
            {
                FProcessAutomatedIntranetExportEnabled = value;
            }
        }

        /// <summary>
        /// Processes the Partner Reminders.
        /// </summary>
        private static void ProcessRemindersDelegate(object AState)
        {
            TLogging.Log("TODO: Automated partner reminders have not been implemented yet");
            return;
#if TODO
            TDataBase db = EstablishDBConnection();

            TProcessPartnerReminders.Process(db);

            CloseDBConnection(db);

            if (TLogging.DebugLevel >= 9)
            {
                TLogging.Log("TTimedProcessing.ProcessRemindersDelegate has run.");
            }
#endif
        }

        /// <summary>
        /// Processes the Automated Intranet (Caleb) Export.
        /// </summary>
        private static void ProcessAutomatedIntranetExportDelegate(object AState)
        {
            TLogging.Log("TODO: Automated Export to the Intranet has not been implemented yet");
            return;
#if TODO
            TDataBase db = EstablishDBConnection();

            TProcessAutomatedIntranetExport.Process(db);

            CloseDBConnection(db);

            if (TLogging.DebugLevel >= 9)
            {
                TLogging.Log("TTimedProcessing.ProcessAutomatedIntranetExportDelegate has run.");
            }
#endif
        }

        /// <summary>
        /// Establishes a new Database connection to the Database
        /// for TTimedProcessing.
        /// </summary>
        /// <remarks>
        /// We don't want to use the global Ict.Common.DB.DBAccess.GDBAccessObj object in the Default
        /// AppDomain because this is reserved for OpenPetraServer's internal use (eg. verifying Client
        /// connection reqests)!
        /// </remarks>
        /// <returns>the database connection object</returns>
        private static TDataBase EstablishDBConnection()
        {
            TDataBase FDBAccessObj = new Ict.Common.DB.TDataBase();

            try
            {
                FDBAccessObj.EstablishDBConnection(TSrvSetting.RDMBSType,
                    TSrvSetting.PostgreSQLServer,
                    TSrvSetting.PostgreSQLServerPort,
                    TSrvSetting.PostgreSQLDatabaseName,
                    TSrvSetting.DBUsername,
                    TSrvSetting.DBPassword,
                    "");
            }
            catch (Exception)
            {
                /* TLogging.Log('Exception occured while establishing connection to Database Server: ' + exp.ToString); */
                throw;
            }

            return FDBAccessObj;
        }

        /// <summary>
        /// Closes the Database connection to the Database
        /// for TTimedProcessing.
        /// </summary>
        /// <returns>void</returns>
        private static void CloseDBConnection(TDataBase DBAccessObj)
        {
            DBAccessObj.CloseDBConnection();
        }

        /// <summary>
        /// Starts all processing Timers.
        /// </summary>
        /// <description>This Method performs immediate processing
        /// if the time where processing is supposed to start is already in
        /// the past.</description>
        public static void StartProcessing()
        {
            DateTime TodaysStartTime;
            DateTime TomorrowsStartTime;
            TimeSpan InitialSleepTime;
            TimeSpan TwentyfourHrs;

            // Check if any Processing is enabled at all
            if (!(FProcessPartnerRemindersEnabled || FProcessAutomatedIntranetExportEnabled))
            {
                // No Processing is enabled, therefore we don't do anything here!
                return;
            }

            // Set up the Delegates that will be passed to the Timer
            if (FProcessPartnerRemindersEnabled)
            {
                FRemindersProcessor = new TRemindersProcessor(ProcessRemindersDelegate);
            }

            if (FProcessAutomatedIntranetExportEnabled)
            {
                FIntranetExportProcessor = new TAutomatedIntranetExportProcessor(ProcessAutomatedIntranetExportDelegate);
            }

            /*
             * Calculate the Timer's time periods
             */

            // Calculate the DateTime of the processing time of today
            TodaysStartTime = DateTime.Now.Date.Add(
                new TimeSpan(FDailyStartTime24Hrs.Hour, FDailyStartTime24Hrs.Minute, 0));

            // Calculate the DateTime of the processing time of the following day
            TomorrowsStartTime = TodaysStartTime.AddDays(1);

            // Calculate the time that the Timer should sleep until it wakes up on processing time of the following day
            InitialSleepTime = TomorrowsStartTime.Subtract(DateTime.Now);

            // Create a TimeSpan that is 1 day (=24 hrs). This is the interval in which following Timer wakeups will occur
            TwentyfourHrs = new TimeSpan(1, 0, 0, 0);             // = 1 day

            if (TLogging.DebugLevel >= 9)
            {
                TLogging.Log("TTimedProcessing.StartProcessing: TodaysStartTime: " + TodaysStartTime.ToString());
                TLogging.Log("TTimedProcessing.StartProcessing: TomorrowsStartTime: " + TomorrowsStartTime.ToString());
                TLogging.Log("TTimedProcessing.StartProcessing: InitialSleepTime: " + InitialSleepTime.ToString());
                TLogging.Log("TTimedProcessing.StartProcessing: TomorrowsStartTime + TwentyfourHrs: " +
                    TomorrowsStartTime.AddTicks(TwentyfourHrs.Ticks).ToString());
            }

            /*
             * If the daily start time is earlier that the current time: process individual Processing processes
             * immediately to ensure that they were run today.
             */
            if (TodaysStartTime < DateTime.Now)
            {
                if (FProcessPartnerRemindersEnabled)
                {
                    ProcessRemindersDelegate(null);
                }

                if (FProcessAutomatedIntranetExportEnabled)
                {
                    ProcessAutomatedIntranetExportDelegate(null);
                }
            }

            /*
             * Start the Timer(s) for the individual processing Processes
             */

            if (FProcessPartnerRemindersEnabled)
            {
                /*
                 * Schedule the regular Partner Reminders processing calls.
                 * The Timer will execute the function 'ProcessRemindersDelegate' through
                 * calling the FRemindersProcessor Delegate in the specified time periods.
                 */
                FRemindersTimer = new System.Threading.Timer(
                    new TimerCallback(FRemindersProcessor),
                    null,
                    InitialSleepTime,
                    TwentyfourHrs);
            }

            if (FProcessAutomatedIntranetExportEnabled)
            {
                /*
                 * Schedule the regular Automatic Intranet (Caleb) Export calls.
                 * The Timer will execute the function 'ProcessAutomatedIntranetExportDelegate' through
                 * calling the FIntranetExportProcessor Delegate in the specified time periods.
                 */
                FIntranetExportTimer = new System.Threading.Timer(
                    new TimerCallback(FIntranetExportProcessor),
                    null,
                    InitialSleepTime.Add(new TimeSpan(0, MINUTES_DELAY_BETWEEN_INDIV_PROCESSES, 0)),
                    TwentyfourHrs.Add(new TimeSpan(0, MINUTES_DELAY_BETWEEN_INDIV_PROCESSES, 0)));
            }
        }

        /// <summary>
        /// Manually call the reminders process
        /// </summary>
        public static void ManuallyProcessReminders()
        {
            ProcessRemindersDelegate(null);
        }

        /// <summary>
        /// Manually call the Intranet export
        /// </summary>
        public static void ManuallyProcessIntranetExport()
        {
            ProcessAutomatedIntranetExportDelegate(null);
        }
    }
}