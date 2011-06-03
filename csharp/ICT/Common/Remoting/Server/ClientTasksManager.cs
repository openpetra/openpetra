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
using Ict.Common.Remoting.Shared;

namespace Ict.Common.Remoting.Server
{
    /// <summary>
    /// Handles Server-to-Client messaging.
    ///
    /// Tasks (Messages) can be added. These get queued until the Client makes a
    /// KeepAlive call, whereupon the new Task(s) are returned to the Client. These
    /// Tasks get archived in memory on the Server side (for later use, eg. in a
    /// workflow scenario).
    ///
    /// @todo Not thread save yet!
    ///
    /// </summary>
    public class TClientTasksManager
    {
        /// <summary>DataTable holding added Tasks.</summary>
        private DataTable FClientTasksNewDataTable;

        /// <summary>DataTable holding Tasks that have been fetched by the Client.</summary>
        private DataTable FClientTasksHistoryDataTable;

        /// <summary>
        /// Gets called by KeepAlive to inquire if there are Tasks
        /// that need to be passed on to the Client.
        ///
        /// </summary>
        public Boolean ClientTasksNewDataTableEmpty
        {
            get
            {
                return Get_ClientTasksNewDataTableEmpty();
            }
        }

        /// <summary>
        /// Gets called by KeepAlive to get the Tasks that need to be
        /// passed on to the Client.
        /// Automatically moves these Tasks to the archive DataTable and sets their
        /// status from 'New' to 'Fetched'.
        ///
        /// </summary>
        public DataTable ClientTasksNewDataTable
        {
            get
            {
                return Get_ClientTasksNewDataTable();
            }
        }


        #region TClientTasksManager

        /// <summary>
        /// Initialises the DataTable that hold new and archived tasks (messages).
        ///
        /// </summary>
        /// <returns>void</returns>
        public TClientTasksManager()
        {
            DataColumn PrimaryKeyColumn;

            FClientTasksNewDataTable = new DataTable("ClientTasks");
            PrimaryKeyColumn = FClientTasksNewDataTable.Columns.Add("TaskID", typeof(System.Int64));
            PrimaryKeyColumn.AutoIncrement = true;
            PrimaryKeyColumn.AutoIncrementSeed = 1;
            PrimaryKeyColumn.AutoIncrementStep = 1;
            FClientTasksNewDataTable.Columns.Add("TaskGroup", typeof(String));
            FClientTasksNewDataTable.Columns.Add("TaskCode", typeof(String));
            FClientTasksNewDataTable.Columns.Add("TaskParameter1", typeof(System.Object));
            FClientTasksNewDataTable.Columns.Add("TaskParameter2", typeof(System.Object));
            FClientTasksNewDataTable.Columns.Add("TaskParameter3", typeof(System.Object));
            FClientTasksNewDataTable.Columns.Add("TaskParameter4", typeof(System.Object));
            FClientTasksNewDataTable.Columns.Add("TaskPriority", typeof(System.Int16));
            FClientTasksNewDataTable.Columns.Add("TaskPercentDone", typeof(System.Int16));
            FClientTasksNewDataTable.Columns.Add("TaskStatus", typeof(String));
            DataColumn[] PrimaryKeyArray =
            {
                PrimaryKeyColumn
            };
            FClientTasksNewDataTable.PrimaryKey = PrimaryKeyArray;
            FClientTasksHistoryDataTable = FClientTasksNewDataTable.Clone();
        }

        /// <summary>
        /// Property accessor. Gets called by KeepAlive to inquire if there are Tasks
        /// that need to be passed on to the Client.
        ///
        /// </summary>
        /// <returns>void</returns>
        public Boolean Get_ClientTasksNewDataTableEmpty()
        {
            Boolean ReturnValue;

            if (FClientTasksNewDataTable.Rows.Count == 0)
            {
                ReturnValue = true;

                // Console.WriteLine('get_ClientTasksNewDataTableEmpty: Client Tasks Table is empty!');
            }
            else
            {
                ReturnValue = false;

                // Console.WriteLine('get_ClientTasksNewDataTableEmpty: Client Tasks Table is not empty!');
            }

            return ReturnValue;
        }

        /// <summary>
        /// Adds a Client Task.
        ///
        /// </summary>
        /// <param name="ATaskGroup">Task Group (eg. USERMESSAGE, CACHEREFRESH)</param>
        /// <param name="ATaskCode">Task Code (eg. User message text, Cache table to refresh)</param>
        /// <param name="ATaskParameter1">Task Parameter1 (currently ignored on Client side)</param>
        /// <param name="ATaskParameter2">Task Parameter2 (currently ignored on Client side)</param>
        /// <param name="ATaskParameter3">Task Parameter3 (currently ignored on Client side)</param>
        /// <param name="ATaskParameter4">Task Parameter4 (currently ignored on Client side)</param>
        /// <param name="ATaskPriority">Task Priority (currently ignored on Client side)
        /// @result TaskID
        /// </param>
        /// <returns>void</returns>
        public Int32 ClientTaskAdd(String ATaskGroup,
            String ATaskCode,
            object ATaskParameter1,
            object ATaskParameter2,
            object ATaskParameter3,
            object ATaskParameter4,
            Int16 ATaskPriority)
        {
            DataRow NewEntry;

            if (ATaskParameter1 == null)
            {
                ATaskParameter1 = DBNull.Value;
            }

            if (ATaskParameter2 == null)
            {
                ATaskParameter2 = DBNull.Value;
            }

            if (ATaskParameter3 == null)
            {
                ATaskParameter3 = DBNull.Value;
            }

            if (ATaskParameter4 == null)
            {
                ATaskParameter4 = DBNull.Value;
            }

            NewEntry = FClientTasksNewDataTable.NewRow();
            NewEntry["TaskGroup"] = ATaskGroup;
            NewEntry["TaskCode"] = ATaskCode;
            NewEntry["TaskParameter1"] = ATaskParameter1;
            NewEntry["TaskParameter2"] = ATaskParameter2;
            NewEntry["TaskParameter3"] = ATaskParameter3;
            NewEntry["TaskParameter4"] = ATaskParameter4;
            NewEntry["TaskPriority"] = ATaskPriority.ToString();
            NewEntry["TaskPercentDone"] = '0';
            NewEntry["TaskStatus"] = "New";
            FClientTasksNewDataTable.Rows.Add(NewEntry);
            return Convert.ToInt32(NewEntry["TaskID"]);

            // Console.WriteLine('Added new Task ''' + ATaskCode + '''; TaskID: ' + Result.ToString);
        }

        /// <summary>
        /// Returns the current Status of a Task.
        ///
        /// </summary>
        /// <param name="ATaskID">Task ID
        /// </param>
        /// <returns>void</returns>
        public String ClientTaskStatus(System.Int64 ATaskID)
        {
            String ReturnValue;

            // first search in ClientTasksNewDataTable for the TaskID
            DataRow[] ResultRows = FClientTasksNewDataTable.Select("TaskID = " + ATaskID.ToString());

            if (ResultRows.Length > 0)
            {
                ReturnValue = ResultRows[0]["TaskStatus"].ToString();
            }
            else
            {
                // if not found in ClientTasksNewDataTable, search in ClientTasksHistoryDataTable
                // for the TaskID
                ResultRows = FClientTasksHistoryDataTable.Select("TaskID = " + ATaskID.ToString());

                if (ResultRows.Length > 0)
                {
                    ReturnValue = ResultRows[0]["TaskStatus"].ToString();
                }
                else
                {
                    // Task with TaskID doesn't exist!
                    ReturnValue = "N/A";
                }
            }

            // Console.WriteLine('Task Status for TaskID# ' + Result.ToString + ':' + Result);
            return ReturnValue;
        }

        /// <summary>
        /// Property accessor. Gets called by KeepAlive to get the Tasks that need to be
        /// passed on to the Client.
        /// Automatically moves these Tasks to the archive DataTable and sets their
        /// status from 'New' to 'Fetched'.
        ///
        /// </summary>
        /// <returns>void</returns>
        public DataTable Get_ClientTasksNewDataTable()
        {
            DataTable ReturnValue;
            DataRow FetchedEntryRow;

            // Console.WriteLine('get_ClientTasksNewDataTable called.');
            // TODO 1 ochristiank cClient Tasks : Make this process threadsave.
            ReturnValue = FClientTasksNewDataTable.Copy();

            foreach (DataRow NewEntryRow in FClientTasksNewDataTable.Rows)
            {
                // Console.WriteLine('get_ClientTasksNewDataTable: copying row');
                FetchedEntryRow = FClientTasksHistoryDataTable.NewRow();
                FetchedEntryRow.ItemArray = NewEntryRow.ItemArray;
                FetchedEntryRow["TaskStatus"] = "Fetched";

                // Console.WriteLine('get_ClientTasksNewDataTable: row copied');
                // Console.WriteLine('Task with TaskID# ' + FetchedEntryRow['TaskID'].ToString + ' got moved from New to History table.');
            }

            FClientTasksNewDataTable.Rows.Clear();
            return ReturnValue;
        }

        #endregion
    }
}