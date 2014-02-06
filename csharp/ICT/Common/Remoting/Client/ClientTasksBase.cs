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
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Ict.Common;
using System.Windows.Forms;

namespace Ict.Common.Remoting.Client
{
    /// <summary>
    /// Manages the execution of Client Tasks.
    ///
    /// Tasks are first retrieved by the KeepAlive tread. TClientTasksQueue then
    /// executes the separate Tasks asynchronously using a TClientTaskInstance for
    /// each Task.
    /// </summary>
    public class TClientTasksQueue
    {
        // only needed for debugging
        // private static Int32 UClientID;

        /// Holds the ClientTasksDataTable that was passed in when Create got called.
        private DataTable FClientTasksDataTable;

        /// <summary>
        /// set the type of your derived class for TClientTaskInstance
        /// </summary>
        static public Type ClientTasksInstanceType = typeof(TClientTaskInstanceBase);

        /// <summary>
        /// Passes in the ClientTasksDataTable which contains the Client Tasks.
        /// </summary>
        /// <param name="AClientID"></param>
        /// <param name="AClientTasksDataTable"></param>
        [SuppressMessage("Gendarme.Rules.Performance", "AvoidUnusedParametersRule",
             Justification = "AClientID is used only for debugging and otherwise its use is commented",
             MessageId = "AClientID")]
        public TClientTasksQueue(Int32 AClientID, DataTable AClientTasksDataTable)
        {
            // only needed for debugging
            // UClientID = AClientID;
            FClientTasksDataTable = AClientTasksDataTable;
        }

        /// <summary>
        /// Gets called by KeepAliveThread if new ClientTasks were sent by the Server.
        /// </summary>
        public void QueueClientTasks()
        {
            TClientTaskInstanceBase ClientTaskInstance;
            Thread ClientTaskThread;

            // TODO 2 ochristiank cClient Tasks : Work the queue of Client Tasks according to their Priority
            foreach (DataRow NewEntryRow in FClientTasksDataTable.Rows)
            {
                ClientTaskInstance = (TClientTaskInstanceBase)Activator.CreateInstance(ClientTasksInstanceType);
                ClientTaskInstance.ClientTask = NewEntryRow;
                ClientTaskThread = new Thread(new ThreadStart(ClientTaskInstance.Execute));
                ClientTaskThread.Start();
            }
        }
    }

    /// <summary>
    /// Executes a certain Task.
    /// </summary>
    public class TClientTaskInstanceBase
    {
        /// Holds a certain row of the ClientTasksDataTable that was passed in when ClientTask property got set.
        protected DataRow FClientTaskDataRow;

        /// <summary>
        /// Property for passing the DataRow of a Client Task.
        /// </summary>
        public DataRow ClientTask
        {
            get
            {
                return FClientTaskDataRow;
            }

            set
            {
                FClientTaskDataRow = value;
            }
        }

        /// <summary>
        /// Executes the Client Task.
        /// </summary>
        public virtual void Execute()
        {
            // should be overwritten by a specific class
        }
    }
}