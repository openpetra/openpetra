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
using Ict.Common;
using System.Threading;
using Ict.Petra.Server.App.ClientDomain;

namespace Ict.Petra.Server.App.ClientDomain
{
    /// <summary>Delegate declaration</summary>
    public delegate void TDelegateTearDownAppDomain(String AToken, String AReason);

    /// <summary>
    /// The TClientStillAliveCheck Class monitors whether the connected PetraClient is still 'alive'.
    /// </summary>
    /// <remarks>
    /// If this Class finds out that the connected PetraClient isn't 'alive'
    /// anymore, it will initiate the tearing down of the Client's AppDomain!
    /// </remarks>
    class ClientStillAliveCheck
    {
        #region Resourcestrings

        private static readonly string StrClientFailedToContact = Catalog.GetString(
            "Client failed to contact OpenPetra Server regularly (last contact was {0} ago [Format: hh:mm:ss]): ClientStillAliveCheck mechanism found timeout expired!");

        #endregion

        private static TDelegateTearDownAppDomain UTearDownAppDomain;
        private static String UTearDownAppDomainToken;
        private static Thread UClientStillAliveCheckThread;
        private static Boolean UKeepServerAliveCheck;
        private static Int16 UClientStillAliveTimeout;
        private static Int16 UClientStillAliveCheckInterval;


        /**
         * Monitors whether the connected PetraClient is still 'alive'.
         *
         * This is done using a Thread that checks the Date and Time when the last
         * Client call to 'PollClientTasks' was made.
         *
         * @comment If this Class finds out that the connected PetraClient isn't 'alive'
         * anymore, it will initiate the tearing down of the Client's AppDomain!!!
         *
         */
        public class TClientStillAliveCheck : object
        {
            /**
             * Constructor for passing in parameters.
             *
             * @param AClientServerConnectionType Type of Client connection
             * @param ATearDownAppDomain Delegate that is called once
             * ClientStillAliveCheckThread finds out that the Client is no longer 'alive'
             * @param ATearDownAppDomainToken Security Token. Prevents against unauthorized
             * tearing down of the Client's AppDomain.
             *
             */
            public TClientStillAliveCheck(TClientServerConnectionType AClientServerConnectionType,
                TDelegateTearDownAppDomain ATearDownAppDomain,
                String ATearDownAppDomainToken)
            {
                Int16 ClientStillAliveTimeout;

#if DEBUGMODE
                if (TSrvSetting.DL >= 10)
                {
                    Console.WriteLine("{0} TClientStillAliveCheck created", DateTime.Now);
                }
#endif

                // Determine timeout limit (different for Clients connected via LAN or Remote)
                if (AClientServerConnectionType == TClientServerConnectionType.csctRemote)
                {
                    ClientStillAliveTimeout = TSrvSetting.ClientKeepAliveTimeoutAfterXSecondsRemote;
                }
                else if (AClientServerConnectionType == TClientServerConnectionType.csctLAN)
                {
                    ClientStillAliveTimeout = TSrvSetting.ClientKeepAliveTimeoutAfterXSecondsLAN;
                }
                else
                {
                    ClientStillAliveTimeout = TSrvSetting.ClientKeepAliveTimeoutAfterXSecondsLAN;
                }

                UClientStillAliveTimeout = ClientStillAliveTimeout;
                UClientStillAliveCheckInterval = TSrvSetting.ClientKeepAliveCheckIntervalInSeconds;
                UTearDownAppDomain = ATearDownAppDomain;
                UTearDownAppDomainToken = ATearDownAppDomainToken;

                // Start ClientStillAliveCheckThread
                UKeepServerAliveCheck = true;
                UClientStillAliveCheckThread = new Thread(new ThreadStart(ClientStillAliveCheckThread));
                UClientStillAliveCheckThread.IsBackground = true;
                UClientStillAliveCheckThread.Start();
#if DEBUGMODE
                if (TSrvSetting.DL >= 10)
                {
                    Console.WriteLine("{0} TClientStillAliveCheck: started ClientStillAliveCheckThread.", DateTime.Now);
                }
#endif
            }

            /**
             * Thread that checks in regular intervals whether the Date and Time when the
             * last Client call to 'PollClientTasks' was made exceeds a certain timeout
             * limit (different for Clients connected via LAN or Remote).
             *
             * @comment The Thread is started at Class instantiation and can be stopped by
             * calling the StopClientStillAliveCheckThread method.
             *
             * @comment The check interval can be configured with the ServerSetting
             * 'Server.ClientKeepAliveCheckIntervalInSeconds'.
             *
             */
            public void ClientStillAliveCheckThread()
            {
                TimeSpan Duration;
                DateTime LastPollingTime;

                // Check whether this Thread should still execute
                while (UKeepServerAliveCheck)
                {
#if DEBUGMODE
                    if (TSrvSetting.DL >= 10)
                    {
                        Console.WriteLine("{0} TClientStillAliveCheck: ClientStillAliveCheckThread: checking...", DateTime.Now);
                    }
#endif

                    // Get the time of the last call to TPollClientTasks.PollClientTasks
                    LastPollingTime = TPollClientTasks.GetLastPollingTime();

                    // Calculate time between the last call to TPollClientTasks.PollClientTasks and now
                    Duration = DateTime.Now.Subtract(LastPollingTime);

                    // Determine whether the timeout has been exceeded
                    if (Duration.TotalSeconds < UClientStillAliveTimeout)
                    {
                        // No it hasn't
#if DEBUGMODE
                        if (TSrvSetting.DL >= 10)
                        {
                            Console.WriteLine("{0} TClientStillAliveCheck: ClientStillAliveCheckThread: timeout hasn't been exceeded.", DateTime.Now);
                        }
#endif
                        try
                        {
                            // Sleep for some time. After that, this procedure is called again automatically.
#if DEBUGMODE
                            if (TSrvSetting.DL >= 10)
                            {
                                Console.WriteLine("{0} TClientStillAliveCheck: ClientStillAliveCheckThread: going to sleep...", DateTime.Now);
                            }
#endif
                            Thread.Sleep(UClientStillAliveCheckInterval * 1000);
#if DEBUGMODE
                            if (TSrvSetting.DL >= 10)
                            {
                                Console.WriteLine("{0} TClientStillAliveCheck: ClientStillAliveCheckThread: re-awakening...", DateTime.Now);
                            }
#endif
                        }
                        catch (ThreadAbortException)
                        {
#if DEBUGMODE
                            if (TSrvSetting.DL >= 10)
                            {
                                Console.WriteLine("{0} TClientStillAliveCheck: ClientStillAliveCheckThread: ThreadAbortException occured!!!",
                                    DateTime.Now);
                            }
#endif
                            UKeepServerAliveCheck = false;
                        }
                    }
                    else
                    {
                        // TODO 1 oChristianK cLogging (Console) : Put the following debug messages again in a DEBUGMODE conditional compilation directive and raise the DL to >=10; this was removed to trace problems in on live installations!
                        if (TSrvSetting.DL >= 5)
                        {
                            Console.WriteLine(
                                "{0} TClientStillAliveCheck: ClientStillAliveCheckThread: timeout HAS been exceeded (last PollClientTasks call: " +
                                LastPollingTime.ToString() + ") -> SignalTearDownAppDomain!",
                                DateTime.Now);
                        }

                        /*
                         * Timeout has been exceeded, this means the Client didn't make a call
                         * to TPollClientTasks.PollClientTasks within the time that is specified
                         * in UClientStillAliveTimeout
                         */

                        /*
                         * KeepServerAliveCheck Thread should no longer run (has an effect only
                         * when this procedure is called from the ClientStillAliveCheckThread
                         * Thread itself)
                         */
                        UKeepServerAliveCheck = false;

                        if (UTearDownAppDomain != null)
                        {
                            UTearDownAppDomain(UTearDownAppDomainToken,
                                String.Format(StrClientFailedToContact, Duration.Hours.ToString() + ':' + Duration.Minutes.ToString() + ':' +
                                    Duration.Seconds.ToString()));
                        }
                        else
                        {
#if DEBUGMODE
                            if (TSrvSetting.DL >= 10)
                            {
                                Console.WriteLine(
                                    "{0} TClientStillAliveCheck: FTearDownAppDomain was not assigned -> can't tear down Client's AppDomain!",
                                    DateTime.Now);
                            }
#endif
                        }
                    }
                }

                // Thread stops here and doesn't get called again automatically.
#if DEBUGMODE
                if (TSrvSetting.DL >= 10)
                {
                    Console.WriteLine("{0} TClientStillAliveCheck: ClientStillAliveCheckThread: Thread stopped!", DateTime.Now);
                }
#endif
            }

            /**
             * Stops the ClientStillAliveCheckThread.
             *
             * @comment There is no way to start the ClientStillAliveCheckThread again.
             * Since stopping the Thread is only done when the Client disconnects/is
             * disconnected, re-starting the Thread is not necessary.
             *
             */
            public static void StopClientStillAliveCheckThread()
            {
                Boolean JoinTimeoutNotExceeded;

                if (UClientStillAliveCheckThread != null)
                {
                    // TODO 1 oChristianK cLogging (Console) : Put the following debug messages again in a DEBUGMODE conditional compilation directive and raise the DL to >=10; this was removed to trace problems in on live installations!
                    if (TSrvSetting.DL >= 5)
                    {
                        Console.WriteLine("{0} TClientStillAliveCheck: StopClientStillAliveCheckThread called: aborting Thread!", DateTime.Now);
                    }

                    UClientStillAliveCheckThread.Abort();

                    if (TSrvSetting.DL >= 5)
                    {
                        Console.WriteLine("{0} TClientStillAliveCheck: StopClientStillAliveCheckThread: aborting returned, now Joining...!",
                            DateTime.Now);
                    }

                    // Wait until the Thread is finished Aborting. Continue anyway after 8 seconds if it doesn't (this should'n happen, but...)
                    JoinTimeoutNotExceeded = UClientStillAliveCheckThread.Join(8000);

                    if (TSrvSetting.DL >= 5)
                    {
                        Console.WriteLine(
                            "{0} TClientStillAliveCheck: StopClientStillAliveCheckThread: aborting returned, Join returned - JoinTimeoutNotExceeded: "
                            +
                            JoinTimeoutNotExceeded.ToString(),
                            DateTime.Now);
                    }

                    UClientStillAliveCheckThread = null;
                }
                else
                {
                    if (TSrvSetting.DL >= 5)
                    {
                        Console.WriteLine("{0} TClientStillAliveCheck: StopClientStillAliveCheckThread: Thread doesn't exist any longer.",
                            DateTime.Now);
                    }
                }
            }
        }
    }
}