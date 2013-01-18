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
using System.Threading;
using Ict.Common;
using System.Windows.Forms;
using Ict.Common.Remoting.Client;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.App.Core
{
    /// <summary>
    /// Executes a certain Task.
    /// </summary>
    public class TClientTaskInstance : TClientTaskInstanceBase
    {
        /// <summary>
        /// Executes the Client Task.
        /// </summary>
        public override void Execute()
        {
            try
            {
                // MessageBox.Show('Executing Client Task #' + FClientTaskDataRow['TaskID'].ToString + ' in Thread.');
                if (FClientTaskDataRow["TaskGroup"].ToString() == SharedConstants.CLIENTTASKGROUP_USERMESSAGE)
                {
                    // MessageBox.Show(CLIENTTASKGROUP_USERMESSAGE + ' (Client Task #' + FClientTaskDataRow['TaskID'].ToString + '): ' + FClientTaskDataRow['TaskCode'].ToString, 'Client #' + UClientID.ToString + ' received a ClientTask.');
                    MessageBox.Show(FClientTaskDataRow["TaskCode"].ToString(), "OpenPetra Message");
                }

                if (FClientTaskDataRow["TaskGroup"].ToString() == SharedConstants.CLIENTTASKGROUP_CACHEREFRESH)
                {
                    if (FClientTaskDataRow["TaskParameter1"].ToString() == "")
                    {
                        TDataCache.ReloadCacheTable(FClientTaskDataRow["TaskCode"].ToString());
                    }
                    else
                    {
                        TDataCache.ReloadCacheTable(FClientTaskDataRow["TaskCode"].ToString(), FClientTaskDataRow["TaskParameter1"]);
                    }
                }

                if (FClientTaskDataRow["TaskGroup"].ToString() == SharedConstants.CLIENTTASKGROUP_USERDEFAULTSREFRESH)
                {
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
                    TUserInfo.ReloadCachedUserInfo();
                }

                if (FClientTaskDataRow["TaskGroup"].ToString() == SharedConstants.CLIENTTASKGROUP_DISCONNECT)
                {
                    if (FClientTaskDataRow["TaskCode"].ToString() == "IMMEDIATE")
                    {
                        TLogging.Log("Client disconnected due to server disconnection request.");

                        PetraClientShutdown.Shutdown.SaveUserDefaultsAndDisconnectAndStop();
                    }

                    if (FClientTaskDataRow["TaskCode"].ToString() == "IMMEDIATE-HARDEXIT")
                    {
                        TLogging.Log("Application stopped due to server disconnection request (without saving of User Defaults or disconnection).");

                        // APPLICATION STOPS HERE !!!
                        Environment.Exit(0);
                    }
                }

//                MessageBox.Show("Finished executing Client Task #" + FClientTaskDataRow["TaskID"].ToString() + " in Thread.");
            }
            catch (Exception /* Exp */)
            {
// These lines were previously used in DEBUGMODE:

//              MessageBox.Show("Exception occured in TClientTaskInstance.Execute: \r\n" + Exp.ToString());
//              TLogging.Log("Exception occured in TClientTaskInstance.Execute: \r\n" + Exp.ToString());
            }
        }
    }
}