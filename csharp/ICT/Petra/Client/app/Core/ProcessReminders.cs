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
using System.Threading;
using Ict.Common;

namespace Ict.Petra.Client.App.Core
{
    /// <summary>
    /// Provides means to run the Partner Reminders processing for a PetraClient
    /// that is running in Standalone mode.
    ///
    /// @Comment Partner Reminders processing for PetraClients that are *not* running
    ///   in Standalone mode is done on the Linux server using a cron job that runs
    ///   at midnight everday. For Standalone Clients, a Windows Scheduler job is set
    ///   up that does the Partner Reminders processing, but due to the restriction
    ///   to only one logged in User (with the Standalone License) this job can't
    ///   do the processing while a user is logged in. This is where this Unit comes
    ///   into play - it does the Partner Reminders processing while the PetraClient
    ///   is running and connected to the Progress DB using the same 4GL procedure,
    ///   'autoprocess.p' through AppLink 4GL. TODO this needs to be changed
    /// </summary>
    public class ProcessReminders
    {
        delegate void TRemindersProcessor (System.Object AState);

        private static TRemindersProcessor URemindersProcessor;

        /// <summary>
        /// processing the Partner Reminders.
        ///
        /// </summary>
        /// <returns>void</returns>
        public static void ProcessRemindersDelegate(object AState)
        {
            // todo UCmdMSysMan.ProcessReminders();

            // TLogging.Log('ProcessReminders has run.');
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public static void StartStandaloneRemindersProcessing()
        {
            DateTime Tomorrow00hrs;
            TimeSpan InitialSleepTime;
            TimeSpan TwentyfourHrs;

            // Process the Reminders once immediately
            ProcessRemindersDelegate(null);

            // Set up the Delegate that will be passed to the Timer
            URemindersProcessor = new TRemindersProcessor(ProcessRemindersDelegate);

            /*
             * Calculate the Timer's time periods
             */

            // Calculate the DateTime of midnight of the following day
            Tomorrow00hrs = DateTime.Now.Date.AddDays(1);

            // TLogging.Log('Tomorrow00hrs.ToString: ' + Tomorrow00hrs.ToString);
            // Calculate the time that the Timer should sleep until it wakes up on
            // midnight of the following day.
            InitialSleepTime = Tomorrow00hrs.Subtract(DateTime.Now);

            // TLogging.Log('InitialSleepTime.ToString: ' + InitialSleepTime.ToString);
            // Create a TimeSpan that is 1 day (=24 hrs). This is the interval in which
            // following Timer wakeups will occur
            // = 1 day
            TwentyfourHrs = new TimeSpan(1, 0, 0, 0);

            // TLogging.Log('Tomorrow00hrs + TwentyfourHrs: ' + Tomorrow00hrs.AddTicks(TwentyfourHrs.Ticks).ToString);

            /*
             * Schedule the regular Reminders calls.
             * The Timer will execute the function 'ProcessRemindersDelegate' through
             * calling the URemindersProcessor Delegate in the specified time periods.
             */
            new System.Threading.Timer(new TimerCallback(URemindersProcessor), null, InitialSleepTime, TwentyfourHrs);
        }
    }
}