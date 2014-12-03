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
using Ict.Common;
using Ict.Common.Remoting.Shared;

namespace Ict.Common.Remoting.Server
{
    /**
     * The TPollClientTasks Class contains a Method that returns a DataTable that
     * contains ClientTasks for the currently connected Client.
     */
    public class TPollClientTasks
    {
        /// <summary>Holds a reference to the ClientTasksManager</summary>
        private TClientTasksManager FClientTasksManager;

        /// <summary>Holds Date and Time when the last Client call to 'PollClientTasks' was made.</summary>
        private DateTime FLastPollingTime;

        /// <summary>
        /// access polling time
        /// </summary>
        /// <returns></returns>
        public DateTime GetLastPollingTime()
        {
            return FLastPollingTime;
        }

        /// <summary>
        /// constructor
        /// </summary>
        public TPollClientTasks(TClientTasksManager AClientTasksManager)
        {
            FLastPollingTime = DateTime.Now;
            FClientTasksManager = AClientTasksManager;

            if (TLogging.DL >= 10)
            {
                Console.WriteLine("{0} TPollClientTasks created", DateTime.Now);
            }
        }

        /**
         * Called by the Client to obtain a DataTable that contains ClientTasks.
         *
         * @comment This Method needs to be called in regular intervals by a Thread of
         * the Client to prevent the Client's AppDomain from being teared down by
         * TClientStillAliveCheck!
         *
         * @return DataTable containing the ClientTasks for the connected Client, or
         * nil in case there are no ClientTasks for the connected Client.
         *
         */
        public DataTable PollClientTasks()
        {
            DataTable ReturnValue = null;

//            if (TLogging.DL >= 10)
//            {
            TLogging.LogAtLevel(4, "TPollClientTasks: PollClientTasks called");
//            }

            FLastPollingTime = DateTime.Now;

            // Check whether new ClientTasks should be transferred to the Client
            if (FClientTasksManager.ClientTasksNewDataTableEmpty)
            {
                // This argument is set to null instead of transfering an empty DataTable to
                // reduce the number of bytes that are transfered to the Client!
                ReturnValue = null;

//                if (TLogging.DL > 9)
//                {
                TLogging.LogAtLevel(4, "TPollClientTasks: Client Tasks Table is empty!");
//                }
            }
            else
            {
                // Retrieve new ClientTasks DataTable and pass it on the the Client
                ReturnValue = FClientTasksManager.ClientTasksNewDataTable;

//                if (TLogging.DL >= 9)
//                {
                TLogging.LogAtLevel(4, "TPollClientTasks: Client Tasks Table has " + (ReturnValue.Rows.Count).ToString() + " entries!");
//                }
            }

            return ReturnValue;
        }
    }
}