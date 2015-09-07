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
using System.Diagnostics.CodeAnalysis;
using System.Threading;

using Ict.Common;
using Ict.Common.Session;

namespace Ict.Common.Remoting.Server
{
    /// <summary>Delegate declaration</summary>
    public delegate bool TDelegateTearDownAppDomain(System.Int32 AClientID, String AReason, out String ACantDisconnectReason);

    /// <summary>
    /// The ClientStillAliveCheck Class monitors whether the connected PetraClient is still 'alive'.
    /// </summary>
    /// <remarks>
    /// If this Class finds out that the connected client isn't 'alive'
    /// anymore, it will close the session of the client
    /// </remarks>
    public class ClientStillAliveCheck
    {
        #region Resourcestrings

        private static readonly string StrClientFailedToContact = Catalog.GetString(
            "Client failed to contact OpenPetra Server regularly (last contact was {0} ago [Format: hh:mm:ss]): ClientStillAliveCheck mechanism found timeout expired!");

        #endregion

        [SuppressMessage("Gendarme.Rules.Performance", "AvoidUnusedPrivateFieldsRule",
             Justification = "Gendarme identifies this Field as unused, which is wrong, hence we want to surpress the Gendarme Warning.")]
        private static TDelegateTearDownAppDomain UTearDownAppDomain;

//        [SuppressMessage("Gendarme.Rules.Performance", "AvoidUnusedPrivateFieldsRule",
//             Justification = "Gendarme identifies this Field as unused, which is wrong, hence we want to surpress the Gendarme Warning.")]
//        private static String UTearDownAppDomainToken;

        [SuppressMessage("Gendarme.Rules.Performance", "AvoidUnusedPrivateFieldsRule",
             Justification = "Gendarme identifies this Field as unused, which is wrong, hence we want to surpress the Gendarme Warning.")]
        private static Thread UClientStillAliveCheckThread;

        [SuppressMessage("Gendarme.Rules.Performance", "AvoidUnusedPrivateFieldsRule",
             Justification = "Gendarme identifies this Field as unused, which is wrong, hence we want to surpress the Gendarme Warning.")]
        private static Boolean UKeepServerAliveCheck;

        [SuppressMessage("Gendarme.Rules.Performance", "AvoidUnusedPrivateFieldsRule",
             Justification = "Gendarme identifies this Field as unused, which is wrong, hence we want to surpress the Gendarme Warning.")]
        private static Int32 UClientStillAliveTimeout;

        [SuppressMessage("Gendarme.Rules.Performance", "AvoidUnusedPrivateFieldsRule",
             Justification = "Gendarme identifies this Field as unused, which is wrong, hence we want to surpress the Gendarme Warning.")]
        private static Int32 UClientStillAliveCheckInterval;


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
        public class TClientStillAliveCheck
        {
            private TConnectedClient FClientObject;
            private string ClientName;

            /// <summary>
            /// Constructor for passing in parameters.
            /// </summary>
            public TClientStillAliveCheck(TConnectedClient AConnectedClient,
                TClientServerConnectionType AClientServerConnectionType,
                TDelegateTearDownAppDomain ATearDownAppDomain)
            {
                FClientObject = AConnectedClient;
                ClientName = FClientObject.ClientName;
                Int32 ClientStillAliveTimeout;

                TLogging.LogAtLevel(2, "TClientStillAliveCheck (for ClientName '" + ClientName + "'') created");

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
//                UTearDownAppDomainToken = ATearDownAppDomainToken;


                TLogging.LogAtLevel(2, "ClientStillAliveTimeout: " + ClientStillAliveTimeout.ToString() + "; " +
                    "ClientKeepAliveCheckIntervalInSeconds: " + UClientStillAliveCheckInterval.ToString());

                // Start ClientStillAliveCheckThread
                UKeepServerAliveCheck = true;
                UClientStillAliveCheckThread = new Thread(new ThreadStart(ClientStillAliveCheckThread));
                UClientStillAliveCheckThread.Name = "ClientStillAliveCheckThread" + Guid.NewGuid().ToString();
                UClientStillAliveCheckThread.IsBackground = true;
                UClientStillAliveCheckThread.Start();

                TLogging.LogAtLevel(2, "TClientStillAliveCheck (for ClientName '" + ClientName + "'): started ClientStillAliveCheckThread.");
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
                    TLogging.LogAtLevel(2, "TClientStillAliveCheck (for ClientName '" + ClientName + "'): ClientStillAliveCheckThread: checking...");

                    // Get the time of the last call to TPollClientTasks.PollClientTasks
                    LastPollingTime = FClientObject.FPollClientTasks.GetLastPollingTime();

                    // Calculate time between the last call to TPollClientTasks.PollClientTasks and now
                    Duration = DateTime.Now.Subtract(LastPollingTime);

                    // Determine whether the timeout has been exceeded
                    if (Duration.TotalSeconds < UClientStillAliveTimeout)
                    {
                        // No it hasn't
                        TLogging.LogAtLevel(
                            2,
                            "TClientStillAliveCheck (for ClientName '" + ClientName +
                            "'): ClientStillAliveCheckThread: timeout hasn't been exceeded (Clients' last PollClientTasks was called " +
                            Duration.TotalSeconds.ToString() + " ago).");

                        try
                        {
                            // Sleep for some time. After that, this procedure is called again automatically.
                            TLogging.LogAtLevel(2,
                                "TClientStillAliveCheck (for ClientName '" + ClientName + "'): ClientStillAliveCheckThread: going to sleep...");

                            Thread.Sleep(UClientStillAliveCheckInterval * 1000);

                            TLogging.LogAtLevel(12,
                                "TClientStillAliveCheck (for ClientName '" + ClientName + "'): ClientStillAliveCheckThread: re-awakening...");
                        }
                        catch (ThreadAbortException)
                        {
                            TLogging.LogAtLevel(
                                2,
                                "TClientStillAliveCheck (for ClientName '" + ClientName +
                                "'): ClientStillAliveCheckThread: ThreadAbortException occured!!!");

                            UKeepServerAliveCheck = false;
                        }
                    }
                    else
                    {
                        TLogging.LogAtLevel(
                            1,
                            "TClientStillAliveCheck (for ClientName '" + ClientName +
                            "'): ClientStillAliveCheckThread: timeout HAS been exceeded (last PollClientTasks call: " +
                            LastPollingTime.ToString() + ") -> forcefully disconnecting the Client!");

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

                        // Forcefully disconnect the Client!
                        if (UTearDownAppDomain != null)
                        {
                            string CantDisconnectReason;

                            UTearDownAppDomain(FClientObject.ClientID,
                                String.Format(StrClientFailedToContact, Duration.Hours.ToString() + ':' + Duration.Minutes.ToString() + ':' +
                                    Duration.Seconds.ToString()), out CantDisconnectReason);
                        }
                        else
                        {
                            if (TLogging.DL >= 10)
                            {
                                TLogging.Log("TClientStillAliveCheck: FTearDownAppDomain was not assigned -> can't tear down Client's AppDomain!");
                            }
                        }
                    }
                }

                // Thread stops here and doesn't get called again automatically.
                TLogging.LogAtLevel(12, "TClientStillAliveCheck (for ClientName '" + ClientName + "'): ClientStillAliveCheckThread: Thread stopped!");
            }

            /// <summary>
            /// Causes the KeepServerAliveCheck Thread to no longer execute anything the next time it 'wakes up',
            /// and to end then.
            /// </summary>
            public static void StopClientStillAliveCheckThread()
            {
                UKeepServerAliveCheck = false;
            }
        }
    }
}