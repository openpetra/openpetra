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
using System.Data;
using System.Threading;
using Ict.Common;
using System.Windows.Forms;
using Ict.Petra.Shared;
using Ict.Petra.Client.App.Core;

namespace Ict.Petra.Client.App.Core
{
    /// <summary>
    /// Manages the execution of Client Tasks.
    ///
    /// Tasks are first retrieved by the KeepAlive tread. TClientTasksQueue then
    /// executes the separate Tasks asynchronously using a TClientTaskInstance for
    /// each Task.
    /// </summary>
    public class TClientTasksQueue : object
    {
        // only needed for debugging
        // private static Int32 UClientID;

        /// Holds the ClientTasksDataTable that was passed in when Create got called.
        private DataTable FClientTasksDataTable;

        #region TClientTasks

        /// <summary>
        /// Passes in the ClientTasksDataTable which contains the Client Tasks.
        /// </summary>
        /// <param name="AClientID"></param>
        /// <param name="AClientTasksDataTable"></param>
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
            TClientTaskInstance ClientTaskInstance;
            Thread ClientTaskThread;

            // TODO 2 ochristiank cClient Tasks : Work the queue of Client Tasks according to their Priority
            foreach (DataRow NewEntryRow in FClientTasksDataTable.Rows)
            {
                ClientTaskInstance = new TClientTaskInstance();
                ClientTaskInstance.ClientTask = NewEntryRow;
                ClientTaskThread = new Thread(new ThreadStart(ClientTaskInstance.Execute));
                ClientTaskThread.Start();
            }
        }

        #endregion
    }

    /// <summary>
    /// Executes a certain Task.
    /// </summary>
    public class TClientTaskInstance
    {
        /// Holds a certain row of the ClientTasksDataTable that was passed in when ClientTask property got set.
        private DataRow FClientTaskDataRow;

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


        #region TClientTaskInstance

        /// <summary>
        /// Executes the Client Task.
        /// </summary>
        public void Execute()
        {
            try
            {
                // messagebox.show('Executing Client Task #' + FClientTaskDataRow['TaskID'].ToString + ' in Thread.');
                if (FClientTaskDataRow["TaskGroup"].ToString() == SharedConstants.CLIENTTASKGROUP_USERMESSAGE)
                {
                    // MessageBox.Show(CLIENTTASKGROUP_USERMESSAGE + ' (Client Task #' + FClientTaskDataRow['TaskID'].ToString + '): ' + FClientTaskDataRow['TaskCode'].ToString, 'Client #' + UClientID.ToString + ' received a ClientTask.');
                    MessageBox.Show(FClientTaskDataRow["TaskCode"].ToString(), "Petra Message");
                }

                if (FClientTaskDataRow["TaskGroup"].ToString() == SharedConstants.CLIENTTASKGROUP_CACHEREFRESH)
                {
                    /* $IFDEF DEBUGMODE MessageBox.Show(CLIENTTASKGROUP_CACHEREFRESH + ' (Client Task #' + FClientTaskDataRow['TaskID'].ToString + '): ' + FClientTaskDataRow['TaskCode'].ToString, 'Client #' + UClientID.ToString + ' received a
                     *ClientTask.'); $ENDIF */
                    if (FClientTaskDataRow["TaskParameter1"].ToString() == "")
                    {
                        TDataCache.ReloadCacheTable(FClientTaskDataRow["TaskCode"].ToString());
                    }
                    else
                    {
                        // $IFDEF DEBUGMODE MessageBox.Show(CLIENTTASKGROUP_CACHEREFRESH + ' (Client Task #' + FClientTaskDataRow['TaskID'].ToString + '): TaskParameter1=' + FClientTaskDataRow['TaskParameter1'].ToString); $ENDIF
                        TDataCache.ReloadCacheTable(FClientTaskDataRow["TaskCode"].ToString(), FClientTaskDataRow["TaskParameter1"]);
                    }
                }

                if (FClientTaskDataRow["TaskGroup"].ToString() == SharedConstants.CLIENTTASKGROUP_SYSTEMDEFAULTSREFRESH)
                {
                    // $IFDEF DEBUGMODE MessageBox.Show(CLIENTTASKGROUP_SYSTEMDEFAULTSREFRESH + ' (Client Task #' + FClientTaskDataRow['TaskID'].ToString + ')', 'Client #' + UClientID.ToString + ' received a ClientTask.'); $ENDIF
                    TSystemDefaults.ReloadCachedSystemDefaults();
                }

                if (FClientTaskDataRow["TaskGroup"].ToString() == SharedConstants.CLIENTTASKGROUP_USERDEFAULTSREFRESH)
                {
                    // $IFDEF DEBUGMODE MessageBox.Show(CLIENTTASKGROUP_USERDEFAULTSREFRESH + ' (Client Task #' + FClientTaskDataRow['TaskID'].ToString + ')', 'Client #' + UClientID.ToString + ' received a ClientTask.'); $ENDIF
                    if (FClientTaskDataRow["TaskCode"].ToString() == "All")
                    {
                        // MessageBox.Show('FClientTaskDataRow[''TaskCode''] = All!');
                        TUserDefaults.ReloadCachedUserDefaults();
                        TUserDefaults.SaveChangedUserDefaults();
                    }
                    else
                    {
                        // MessageBox.Show('FClientTaskDataRow[''TaskCode''] <> All, but ''' + FClientTaskDataRow['TaskCode'].ToString + '''');
                        // MessageBox.Show('FClientTaskDataRow[''TaskParameter1'']: ' + FClientTaskDataRow['TaskParameter1'].ToString + "\r\n" +
                        // 'FClientTaskDataRow[''TaskParameter2'']: ' + FClientTaskDataRow['TaskParameter2'].ToString + "\r\n" +
                        // 'FClientTaskDataRow[''TaskParameter3'']: ' + FClientTaskDataRow['TaskParameter3'].ToString);
                        TUserDefaults.RefreshCachedUserDefault(
                            FClientTaskDataRow["TaskParameter1"].ToString(), FClientTaskDataRow["TaskParameter2"].ToString(),
                            FClientTaskDataRow["TaskParameter3"].ToString());
                    }
                }

                if (FClientTaskDataRow["TaskGroup"].ToString() == SharedConstants.CLIENTTASKGROUP_USERINFOREFRESH)
                {
                    // $IFDEF DEBUGMODE MessageBox.Show(CLIENTTASKGROUP_USERINFOREFRESH + ' (Client Task #' + FClientTaskDataRow['TaskID'].ToString + ')', 'Client #' + UClientID.ToString + ' received a ClientTask.'); $ENDIF
                    TUserInfo.ReloadCachedUserInfo();
                }

                // messagebox.show('Finished executing Client Task #' + FClientTaskDataRow['TaskID'].ToString + ' in Thread.');
            }
            catch (Exception Exp)
            {
#if DEBUGMODE
                MessageBox.Show("Exception occured in TClientTaskInstance.Execute: \r\n" + Exp.ToString());
                TLogging.Log("Exception occured in TClientTaskInstance.Execute: \r\n" + Exp.ToString());
#endif
            }
        }

        #endregion
    }
}