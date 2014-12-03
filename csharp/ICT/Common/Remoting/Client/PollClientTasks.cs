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
using System.Data;
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Threading;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Remoting.Shared;

namespace Ict.Common.Remoting.Client
{
    /// <summary>
    /// Creates a Thread which calls a Method of the server-side Class
    /// TPollClientTasks in regular intervals to retrieve any ClientTasks.
    ///
    /// If this signal is no longer received by the PetraServer, the PetraServer
    /// automatically tears down the Client's AppDomain and the Server can no longer
    /// be reached from the Client!
    /// THEREFORE THIS THREAD MUST RUN UNINTERRUPTED AS LONG AS THE CLIENT RUNS TO
    /// ALLOW ACCESS TO THE PETRA SERVER!
    ///
    /// </summary>
    public class TPollClientTasks
    {
        /// <summary>The ClientID of the logged in Client</summary>
        private Int32 FClientID;

        /// <summary>Needs to be true as long as the thread should still execute</summary>
        private bool FKeepPollingClientTasks;

        #region TPollClientTasks

        /// <summary>
        /// Starts the PollClientTasksThread.
        ///
        /// </summary>
        /// <param name="AClientID">ClientID of the PetraClient</param>
        public TPollClientTasks(Int32 AClientID)
        {
            Thread TheThread;

            FClientID = AClientID;
            FKeepPollingClientTasks = true;

            // Start PollClientTasksThread
            TheThread = new Thread(new ThreadStart(PollClientTasksThread));
            TheThread.Name = "PollClientTasksThread" + Guid.NewGuid().ToString();
            TheThread.Start();
        }

        /// <summary>
        /// Stops the PollClientTasksThread.
        ///
        /// @comment The only way to start the PollClientTasksThread again is to create a new
        /// TPollClientTasks object. However, since there should be only one such
        /// thread throughout the Client's lifetime, this should not be necessary.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void StopPollClientTasks()
        {
            // Through this PollClientTasksThread will stop when it awakes next time
            FKeepPollingClientTasks = false;
        }

        /// <summary>
        /// poll the client tasks from the server, and let the server know that this client is still connected
        /// </summary>
        private DataTable RemotePollClientTasks()
        {
            DataTable ResultDT = null;

            // TODORemoting --- The following call breaks NUnitForms Tests 'at random'!
            TLogging.LogAtLevel(4, "RemotePollClientTasks: About to call SessionManager.PollClientTasks...");
            ResultDT = (DataTable)THttpConnector.CallWebConnector("SessionManager", "PollClientTasks", null, "binary")[0];
            TLogging.LogAtLevel(4, "RemotePollClientTasks: Finished calling SessionManager.PollClientTasks...");

            if (ResultDT != null)
            {
                TLogging.LogAtLevel(4, "RemotePollClientTasks: ResultDT has " + ResultDT.Rows.Count.ToString() + " rows!");
            }

            return ResultDT;
        }

        /// <summary>
        /// Thread that calls a Method of the server-side Class TPollClientTasks in
        /// regular intervals.
        ///
        /// @comment The Thread is started at Class instantiation and can be stopped by
        /// calling the StopKeepAlive method.
        ///
        /// @comment The interval can be configured with the ClientSetting
        /// 'ServerPollIntervalInSeconds'.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void PollClientTasksThread()
        {
            DataTable ClientTasksDataTable;
            TClientTasksQueue ClientTasksQueueInstance;
            Thread ClientTaskQueueThread;

            // Check whether this Thread should still execute
            while (FKeepPollingClientTasks)
            {
                try
                {
                    // Make PollClientTasks call to Server to keep the Client's remoted objects
                    // and it's AppDomain alive.
                    // The value of the AClientTasksDataTable parameter is always null, except when
                    // the Server has a queued ClientTask that the Client needs to read.
                    ClientTasksDataTable = RemotePollClientTasks();

                    if (ClientTasksDataTable != null)
                    {
                        TLogging.LogAtLevel(4, "Client Tasks Table has " + ClientTasksDataTable.Rows.Count.ToString() + " entries!");

                        // Queue new ClientTasks and execute them.
                        // This is done in a separate Thread to make sure the PollClientTasks thread can run
                        // without the risk of being interrupted!
                        ClientTasksQueueInstance = new TClientTasksQueue(FClientID, ClientTasksDataTable);
                        ClientTaskQueueThread = new Thread(new ThreadStart(ClientTasksQueueInstance.QueueClientTasks));
                        ClientTaskQueueThread.Name = "ClientTaskQueueThread" + Guid.NewGuid().ToString();
                        ClientTaskQueueThread.Start();
                    }
                }
                catch (System.Runtime.Remoting.RemotingException Exp)
                {
                    // string DebugInfo = StrConnectionUnavailableCause + Exp.ToString();
                    // MessageBox.Show(AppCoreResourcestrings.StrConnectionBroken + DebugInfo, AppCoreResourcestrings.StrConnectionBrokenTitle,
                    //     MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TLogging.Log("RemotingException in TPollClientTasks.PollClientTasksThread: " + Exp.ToString(), TLoggingType.ToLogfile);
                }
                catch (System.Net.Sockets.SocketException Exp)
                {
                    // string DebugInfo = StrConnectionUnavailableCause + Exp.ToString() + "\r\n\r\nSocketException.ErrorCode: " + Exp.ErrorCode.ToString();
                    // MessageBox.Show(AppCoreResourcestrings.StrConnectionClosed + DebugInfo, AppCoreResourcestrings.StrConnectionClosedTitle,
                    //     MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TLogging.Log("SocketException in TPollClientTasks.PollClientTasksThread: " + Exp.ToString(), TLoggingType.ToLogfile);
                }
                catch (Exception Exp)
                {
                    TLogging.Log("Exception in TPollClientTasks.PollClientTasksThread: " + Exp.ToString(), TLoggingType.ToLogfile);

                    if (Exp.Message == THTTPUtils.SESSION_ALREADY_CLOSED)
                    {
                        // TODORemoting close the client

                        TLogging.Log("TPollClientTasks: Should have closed the Client here!!!");

                        return;
                    }
                }

                // Sleep for some time. After that, this function is called again automatically.
                TLogging.LogAtLevel(10, "PollClientTasks sleeping for " + TClientSettings.ServerPollIntervalInSeconds + " seconds");
                Thread.Sleep(TimeSpan.FromSeconds(TClientSettings.ServerPollIntervalInSeconds));
            }

            // Thread stops here and doesn't get called again automatically.
        }

        #endregion
    }
}