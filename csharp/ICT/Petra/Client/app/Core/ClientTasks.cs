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
#if DEBUGMODE
            catch (Exception Exp)
            {
                MessageBox.Show("Exception occured in TClientTaskInstance.Execute: \r\n" + Exp.ToString());
                TLogging.Log("Exception occured in TClientTaskInstance.Execute: \r\n" + Exp.ToString());
            }
#else
            catch (Exception)
            {
            }
#endif
        }
    }
}