/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       christiank
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Collections;
using Ict.Common;
using Ict.Common.DB;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Shared.MSysMan.Data.Access;

using System.Data;

namespace Ict.Petra.Server.App.Core.Security
{
    /// <summary>
    /// The TModuleAccessManager class provides provides functions to work with the
    /// Module Access Permissions of a Petra DB.
    /// </summary>
    public class TModuleAccessManager
    {
        /// <summary>
        /// load the modules available to the given user
        /// </summary>
        /// <param name="AUserID"></param>
        /// <returns></returns>
        public static string[] LoadUserModules(String AUserID)
        {
            string[] ReturnValue;
            TDBTransaction ReadTransaction;
            Boolean NewTransaction = false;
            SUserModuleAccessPermissionTable UserModuleAccessPermissionsDT;
            Int32 CounterOverall;
            Int32 CounterAdded;
            ArrayList UserModuleAccessPermissions;

            // ModulesList: string;
            // Counter: Int32;
            try
            {
                ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out NewTransaction);

                if (SUserModuleAccessPermissionAccess.CountViaSUser(AUserID, ReadTransaction) > 0)
                {
                    SUserModuleAccessPermissionAccess.LoadViaSUser(out UserModuleAccessPermissionsDT, AUserID, ReadTransaction);

//TLogging.Log("UserModuleAccessPermissionsDT.Rows.Count - 1: " + (UserModuleAccessPermissionsDT.Rows.Count - 1).ToString());

                    // Dimension the ArrayList with the maximum number of ModuleAccessPermissions first
                    UserModuleAccessPermissions = new ArrayList(UserModuleAccessPermissionsDT.Rows.Count - 1);

                    CounterAdded = 0;

                    for (CounterOverall = 0; CounterOverall <= UserModuleAccessPermissionsDT.Rows.Count - 1; CounterOverall += 1)
                    {
                        if (UserModuleAccessPermissionsDT[CounterOverall].CanAccess)
                        {
//TLogging.Log("UserModuleAccessPermissionsDT[" + CounterOverall.ToString() + "].ModuleId: " + UserModuleAccessPermissionsDT[CounterOverall].ModuleId + ": CounterAdded: " + CounterAdded.ToString());

                            UserModuleAccessPermissions.Add(UserModuleAccessPermissionsDT[CounterOverall].ModuleId);
                            CounterAdded = CounterAdded + 1;
                        }
                    }

                    if (CounterAdded != 0)
                    {
                        // Copy contents of the ArrayList into the ReturnValue
                        ReturnValue = new string[CounterAdded];
                        Array.Copy(UserModuleAccessPermissions.ToArray(), ReturnValue, CounterAdded);

                        // ModulesList := '';
                        //
                        // for Counter := 0 to CounterAdded  1 do
                        // begin
                        // Console.WriteLine('ModulesList: working on Counter ' + Counter.ToString());
                        // ModulesList := ModulesList + UserModuleAccessPermissionsArray[Counter].ToString() + #10#13;
                        // end;
                        // Console.WriteLine(ModulesList);
                    }
                    else
                    {
                        ReturnValue = new string[0];
                    }
                }
                else
                {
                    ReturnValue = new string[0];
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
#if DEBUGMODE
                    if (TSrvSetting.DL >= 8)
                    {
                        Console.WriteLine("TModuleAccessManager.LoadUserModules: committed own transaction.");
                    }
#endif
                }
            }
            return ReturnValue;
        }
    }
}