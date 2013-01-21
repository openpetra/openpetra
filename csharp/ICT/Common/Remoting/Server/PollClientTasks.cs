//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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
using System.Data;
using Ict.Common;
using Ict.Common.Remoting.Shared;

namespace Ict.Common.Remoting.Server
{
    /**
     * The TPollClientTasks Class contains a Method that returns a DataTable that
     * contains ClientTasks for the currently connected Client.
     */
    public class TPollClientTasks : TConfigurableMBRObject, IPollClientTasksInterface
    {
        /// <summary>Holds a reference to the ClientTasksManager</summary>
        public static TClientTasksManager UClientTasksManager;

        /// <summary>Holds Date and Time when the last Client call to 'PollClientTasks' was made.</summary>
        public static DateTime ULastPollingTime;

        /// <summary>
        /// access polling time
        /// </summary>
        /// <returns></returns>
        public static DateTime GetLastPollingTime()
        {
            return ULastPollingTime;
        }

        /// <summary>
        /// constructor
        /// </summary>
        public TPollClientTasks() : base()
        {
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
            DataTable ReturnValue;

            if (TLogging.DL >= 10)
            {
                Console.WriteLine("{0} TPollClientTasks: PollClientTasks called", DateTime.Now);
            }

            ULastPollingTime = DateTime.Now;

            // Check whether new ClientTasks should be transferred to the Client
            if (UClientTasksManager.ClientTasksNewDataTableEmpty)
            {
                // This argument is set to null instead of transfering an empty DataTable to
                // reduce the number of bytes that are transfered to the Client!
                ReturnValue = null;

                if (TLogging.DL > 9)
                {
                    Console.WriteLine("{0} TPollClientTasks: Client Tasks Table is empty!", DateTime.Now);
                }
            }
            else
            {
                // Retrieve new ClientTasks DataTable and pass it on the the Client
                ReturnValue = UClientTasksManager.ClientTasksNewDataTable;

                if (TLogging.DL >= 9)
                {
                    Console.WriteLine("TPollClientTasks: Client Tasks Table has " + (ReturnValue.Rows.Count).ToString() + " entries!");
                }
            }

            return ReturnValue;
        }
    }

    /**
     * Used to pass in a reference to the ClientTasksManager. That is used by
     * the PollClientTasks function. Since the TPollClientTasks Class is re-created
     * with every Client call to PollClientTasks, this helper Class is needed to
     * set a Unit-wide variable.
     *
     */
    public class TPollClientTasksParameters
    {
        /// <summary>
        /// constructors
        /// </summary>
        /// <param name="AClientTasksManager"></param>
        public TPollClientTasksParameters(TClientTasksManager AClientTasksManager)
        {
            if (TLogging.DL >= 10)
            {
                Console.WriteLine("{0} TPollClientTasksParameters created", DateTime.Now);
            }

            TPollClientTasks.UClientTasksManager = AClientTasksManager;
            TPollClientTasks.ULastPollingTime = DateTime.Now;
        }
    }
}