//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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
using System.Globalization;
using System.Threading;
using System.Collections.Generic;

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Exceptions;
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared.Security;
using Ict.Petra.Shared;

namespace Ict.Petra.Server.App.Core
{
    /// <summary>
    /// Provides means to run certain processing routines at timed intervals.
    /// </summary>
    public static class TTimedProcessing
    {
        /// <summary>Resourcestring used for logging purposes.</summary>
        public const string StrAutomaticProcessing = "Automatic Processing";

        private static int MINUTES_DELAY_BETWEEN_INDIV_PROCESSES = 5;
        delegate void TGenericProcessor (object processormethod);
        /// <summary>
        /// delegate for processing
        /// </summary>
        public delegate void TProcessDelegate(TDataBase Database, bool ARunManually);
        private static SortedList <string, TProcessDelegate>FProcessDelegates = new SortedList <string, TTimedProcessing.TProcessDelegate>();
        private static List <System.Threading.Timer>FTimers = new List <Timer>();

        private static DateTime FDailyStartTime24Hrs;

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
                catch (System.FormatException Exc)
                {
                    throw new EOPAppException(
                        "Server Configuration File error: The value supplied for 'Server.Processing.DailyStartTime24Hrs' (" + value +
                        ") isn't a valid time in 24 hours format (leading zeroes are required for hours and minutes between 0-9)", Exc);
                }
            }
        }

        /// <summary>
        /// Processes the delegate.
        /// overload for the thread, does not work with 2 parameters
        /// </summary>
        private static void GenericProcessor(object ADelegateName)
        {
            GenericProcessor(ADelegateName, false);
        }

        /// <summary>
        /// processes the delegate
        /// </summary>
        private static void GenericProcessor(object ADelegateName, bool ARunManually)
        {
            if (!FProcessDelegates.ContainsKey((string)ADelegateName))
            {
                return;
            }

            TDataBase db = EstablishDBConnection();

            TPetraIdentity PetraIdentity = new TPetraIdentity(
                "SYSADMIN", "", "", "", "", DateTime.MinValue,
                DateTime.MinValue, DateTime.MinValue, 0, -1, -1, false, false, false);

            UserInfo.GUserInfo = new TPetraPrincipal(PetraIdentity, null);

            TProcessDelegate TypedDelegate = FProcessDelegates[(string)ADelegateName];

            TypedDelegate(db, ARunManually);

            CloseDBConnection(db);

            if (TLogging.DebugLevel >= 9)
            {
                TLogging.Log("delegate " + ADelegateName + " has run.");
            }
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
                    "",
                    "Servers's DB Connection for TimedProcessing");
            }
            catch (Exception Exc)
            {
                TLogging.Log("Timed Processing: Exception occured while establishing connection to Database Server: " + Exc.ToString());

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
        /// add a new processing job
        /// </summary>
        public static void AddProcessingJob(string ADelegateName, TProcessDelegate ADelegate)
        {
            if (!FProcessDelegates.ContainsKey(ADelegateName))
            {
                FProcessDelegates.Add(ADelegateName, ADelegate);
            }
        }

        /// <summary>
        /// run this job now
        /// </summary>
        /// <param name="ADelegateName"></param>
        public static void RunJobManually(string ADelegateName)
        {
            GenericProcessor(ADelegateName, true);
        }

        /// <summary>
        /// check if that job has been added
        /// </summary>
        public static bool IsJobEnabled(string ADelegateName)
        {
            return FProcessDelegates.ContainsKey(ADelegateName);
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
            if (FProcessDelegates.Count == 0)
            {
                // No Processing is enabled, therefore we don't do anything here!
                return;
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
                foreach (string delegatename in FProcessDelegates.Keys)
                {
                    // run the job
                    GenericProcessor(delegatename);
                }
            }

            /*
             * Start the Timer(s) for the individual processing Processes
             */
            foreach (string delegatename in FProcessDelegates.Keys)
            {
                InitialSleepTime = InitialSleepTime.Add(new TimeSpan(0, MINUTES_DELAY_BETWEEN_INDIV_PROCESSES, 0));
                TwentyfourHrs = TwentyfourHrs.Add(new TimeSpan(0, MINUTES_DELAY_BETWEEN_INDIV_PROCESSES, 0));

                // Schedule the regular processing calls.
                FTimers.Add(new System.Threading.Timer(
                        new TimerCallback(new TGenericProcessor(GenericProcessor)),
                        delegatename,
                        InitialSleepTime,
                        TwentyfourHrs));
            }
        }
    }
}