//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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

namespace Ict.Common.Remoting.Server
{
    /// <summary>
    /// Class for threaded disconnection of Clients.
    ///
    /// </summary>
    public class TDisconnectClientThread
    {
        private TRunningAppDomain FAppDomainEntry;
        private Int32 FClientID;
        private String FReason;

        /// <summary>used for locking the TDisconnectClientThread</summary>
        private static System.Object UAppDomainUnloadMonitor = new System.Object();

        /// <summary>
        /// todoComment
        /// </summary>
        public TRunningAppDomain AppDomainEntry
        {
            get
            {
                return FAppDomainEntry;
            }

            set
            {
                FAppDomainEntry = value;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public Int32 ClientID
        {
            get
            {
                return FClientID;
            }

            set
            {
                FClientID = value;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public String Reason
        {
            get
            {
                return FReason;
            }

            set
            {
                FReason = value;
            }
        }

        #region TDisconnectClientThread

        /// <summary>
        /// todoComment
        /// </summary>
        public void StartClientDisconnection()
        {
            const Int32 UNLOAD_RETRIES = 20;
            Int32 UnloadExceptionCount;
            String ClientInfo;
            Boolean DBConnectionAlreadyClosed;
            Boolean UnloadFinished;

            DBConnectionAlreadyClosed = false;
            UnloadExceptionCount = 0;
            try
            {
                ClientInfo = "'" + FAppDomainEntry.FClientName + "' (ClientID: " + FAppDomainEntry.FClientID.ToString() + ")";

                if (TLogging.DL >= 4)
                {
                    Console.WriteLine(TClientManager.FormatClientList(false));
                    Console.WriteLine(TClientManager.FormatClientList(true));
                }

                if (TLogging.DL >= 4)
                {
                    TLogging.Log("Disconnecting Client " + ClientInfo + " (Reason: " + FReason + ")...",
                        TLoggingType.ToConsole | TLoggingType.ToLogfile);
                }
                else
                {
                    TLogging.Log("Disconnecting Client " + ClientInfo + " (Reason: " + FReason + ")...", TLoggingType.ToLogfile);
                }

                FAppDomainEntry.FClientDisconnectionStartTime = DateTime.Now;
                FAppDomainEntry.FAppDomainStatus = TAppDomainStatus.adsDisconnectingDBClosing;
                try
                {
                    if (TLogging.DL >= 4)
                    {
                        TLogging.Log("Closing Client DB Connection... [Client " + ClientInfo + "]", TLoggingType.ToConsole | TLoggingType.ToLogfile);
                    }

                    try
                    {
                        FAppDomainEntry.ClientAppDomainConnection.CloseDBConnection();
                    }
                    catch (System.ArgumentNullException)
                    {
                        // don't do anything here: this Exception only indicates that the
                        // DB connection was already closed.
                        DBConnectionAlreadyClosed = true;
                    }
                    catch (Exception)
                    {
                        // make sure any other Exception is going to be handled
                        throw;
                    }

                    if (!DBConnectionAlreadyClosed)
                    {
                        if (TLogging.DL >= 4)
                        {
                            TLogging.Log("Closed Client DB Connection. [Client " + ClientInfo + "]", TLoggingType.ToConsole | TLoggingType.ToLogfile);
                        }
                    }
                    else
                    {
                        if (TLogging.DL >= 4)
                        {
                            TLogging.Log("Client DB Connection was already closed. [Client " + ClientInfo + "]",
                                TLoggingType.ToConsole | TLoggingType.ToLogfile);
                        }
                    }
                }
                catch (Exception exp)
                {
                    TLogging.Log(
                        "Error closing Database connection on Client disconnection!" + " [Client " + ClientInfo + "]" + "Exception: " + exp.ToString());
                }

                if (TLogging.DL >= 5)
                {
                    TLogging.Log("  Before calling ClientAppDomainConnection.StopClientAppDomain...  [Client " + ClientInfo + ']',
                        TLoggingType.ToConsole | TLoggingType.ToLogfile);
                }

                FAppDomainEntry.ClientAppDomainConnection.StopClientAppDomain();

                if (TLogging.DL >= 5)
                {
                    TLogging.Log("  After calling ClientAppDomainConnection.StopClientAppDomain...  [Client " + ClientInfo + ']',
                        TLoggingType.ToConsole | TLoggingType.ToLogfile);
                }

                FAppDomainEntry.FAppDomainStatus = TAppDomainStatus.adsDisconnectingAppDomainUnloading;
                UnloadFinished = false;

                while (!UnloadFinished)
                {
                    try
                    {
Retry:                          //             used only for repeating Unload when an Exception happened

                        if (Monitor.TryEnter(UAppDomainUnloadMonitor, 250))
                        {
                            /*
                             * Try to unload the AppDomain. If an Exception occurs, retries are made
                             * after a delay - until a maximum of retries is reached.
                             */
                            if (TLogging.DL >= 5)
                            {
                                Console.WriteLine(
                                    "  Unloading AppDomain '" + FAppDomainEntry.ClientAppDomainConnection.AppDomainName + "' [Client " + ClientInfo +
                                    "]");
                            }

                            try
                            {
                                if (TLogging.DL >= 4)
                                {
                                    TLogging.Log("Unloading Client Session (AppDomain)..." + "' [Client " + ClientInfo + "]",
                                        TLoggingType.ToConsole | TLoggingType.ToLogfile);
                                }

                                // Note for developers: uncomment the following code to see that the retrying of Unload really works in case Exceptions are thrown,,,
                                // if (UnloadExceptionCount < 4)
                                // {
                                //     raise Exception.Create();
                                // }

                                // Unload the AppDomain
                                FAppDomainEntry.ClientAppDomainConnection.Unload();

                                // Everything went fine!
                                if (TLogging.DL >= 4)
                                {
                                    TLogging.Log("Unloaded Client Session (AppDomain)." + "' [Client " + ClientInfo + "]",
                                        TLoggingType.ToConsole | TLoggingType.ToLogfile);
                                }

                                UnloadExceptionCount = 0;
                                FAppDomainEntry.FClientAppDomainConnection = null;
                                FAppDomainEntry.FClientDisconnectionFinishedTime = DateTime.Now;
                                FAppDomainEntry.FAppDomainStatus = TAppDomainStatus.adsStopped;

                                if (TLogging.DL >= 5)
                                {
                                    Console.WriteLine("  AppDomain unloaded [Client " + ClientInfo + "]");
                                }
                            }
                            catch (Exception UnloadException)
                            {
                                // Something went wrong during Unload; log this, wait a bit and
                                // try again!
                                UnloadExceptionCount = UnloadExceptionCount + 1;
                                TLogging.Log(
                                    "Error unloading Client Session (AppDomain) [Client " + ClientInfo + "] on Client disconnection   (try #" +
                                    UnloadExceptionCount.ToString() + ")!  " + "Exception: " + UnloadException.ToString());
                                Monitor.PulseAll(UAppDomainUnloadMonitor);
                                Monitor.Exit(UAppDomainUnloadMonitor);
                                Thread.Sleep(UnloadExceptionCount * 1000);
                            }

                            if ((UnloadExceptionCount != 0) && (UnloadExceptionCount < UNLOAD_RETRIES))
                            {
                                goto Retry;
                            }

                            if (TLogging.DL >= 5)
                            {
                                Console.WriteLine("  AppDomain unloading finished [Client " + ClientInfo + ']');
                            }

                            // Logging: was Unload successful?
                            if (FAppDomainEntry.FAppDomainStatus == TAppDomainStatus.adsStopped)
                            {
                                if (TLogging.DL >= 4)
                                {
                                    TLogging.Log(
                                        "Client " + ClientInfo + " has been disconnected (took " +
                                        FAppDomainEntry.FClientDisconnectionFinishedTime.Subtract(
                                            FAppDomainEntry.FClientDisconnectionStartTime).TotalSeconds.ToString() + " sec).",
                                        TLoggingType.ToConsole | TLoggingType.ToLogfile);
                                }
                                else
                                {
                                    TLogging.Log("Client " + ClientInfo + " has been disconnected.", TLoggingType.ToLogfile);
                                }
                            }
                            else
                            {
                                if (TLogging.DL >= 4)
                                {
                                    TLogging.Log("Client " + ClientInfo + " could not be disconnected!",
                                        TLoggingType.ToConsole | TLoggingType.ToLogfile);
                                }
                                else
                                {
                                    TLogging.Log("Client " + ClientInfo + " could not be disconnected!", TLoggingType.ToLogfile);
                                }
                            }

                            UnloadFinished = true;

                            // Notify any other waiting Threads that they can proceed with the
                            // Unloading of their AppDomain
                            try
                            {
                                Monitor.PulseAll(UAppDomainUnloadMonitor);
                            }
                            catch (System.Threading.SynchronizationLockException)
                            {
                                if (UnloadExceptionCount == UNLOAD_RETRIES)
                                {
                                }
                                // ignore this Exception if the amount of retries is reached
                                // the Monitor is already Exited in this case
                                else
                                {
                                    throw;
                                }
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                        }
                        else
                        {
                            if (TLogging.DL >= 4)
                            {
                                TLogging.Log(
                                    "Client disconnection Thread is blocked by another client disconnection thread. Waiting a bit... (ClientID: " +
                                    AppDomainEntry.FClientID.ToString() + ")...",
                                    TLoggingType.ToConsole | TLoggingType.ToLogfile);
                            }

                            Thread.Sleep(2000);

                            if (TLogging.DL >= 4)
                            {
                                TLogging.Log(
                                    "Trying to continue Client disconnection after Thread was blocked by another client disconnection thread..." +
                                    "' (ClientID: " + AppDomainEntry.FClientID.ToString() + ")...",
                                    TLoggingType.ToConsole | TLoggingType.ToLogfile);
                            }
                        }
                    }
                    finally
                    {
                        Monitor.Exit(UAppDomainUnloadMonitor);
                    }
                }
            }
            catch (Exception Exp)
            {
                TLogging.Log(
                    "StartClientDisconnection for ClientID: " + FClientID.ToString() + ": Exception occured: " + Exp.ToString(),
                    TLoggingType.ToConsole |
                    TLoggingType.ToLogfile);
            }
        }

        #endregion
    }
}