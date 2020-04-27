//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2020 by OM International
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
using System.Data.Odbc;
using System.Threading;
using Ict.Common;
using Ict.Common.Data.Exceptions;
using Ict.Common.Session;
using Ict.Common.DB;
using Ict.Common.Exceptions;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MSysMan.Data.Access;
using Ict.Common.Data;
using Ict.Petra.Shared;
using Ict.Petra.Server.App.Core.Security;

namespace Ict.Petra.Server.MSysMan.Common.WebConnectors
{
    /// <summary>
    /// Reads and saves User Defaults.
    ///
    /// </summary>
    public class TUserDefaults
    {
        /*------------------------------------------------------------------------------
         *  Finance User Default Constants
         * -------------------------------------------------------------------------------*/

        /// <summary>todoComment</summary>
        public const String FINANCE_DEFAULT_LEDGERNUMBER = "a_default_ledger_number_i";

        /// <summary>
        /// Find out if a user default exists already.
        /// This is required where the default should be calculated otherwise
        /// (e.g. FINANCE_REPORTING_SHOWDIFFFINANCIALYEARSELECTION)
        /// </summary>
        /// <returns>true if a default with the given key already exists
        /// </returns>
        [NoRemoting]
        public static bool HasDefault(String AKey, TDataBase ADataBase = null)
        {
            return GetUserDefault(AKey, SharedConstants.SYSDEFAULT_NOT_FOUND, ADataBase) != SharedConstants.SYSDEFAULT_NOT_FOUND;
        }

        /// <summary>
        /// get boolean default
        /// </summary>
        /// <param name="AKey"></param>
        /// <param name="ADefault"></param>
        /// <param name="ADataBase">database object</param>
        /// <returns>true if key does not exist</returns>
        [NoRemoting]
        public static bool GetBooleanDefault(String AKey, bool ADefault = true, TDataBase ADataBase = null)
        {
            return Convert.ToBoolean(GetUserDefault(AKey, ADefault.ToString()));
        }

        /// <summary>
        /// get char default
        /// </summary>
        /// <param name="AKey"></param>
        /// <param name="ADefault"></param>
        /// <param name="ADataBase">database object</param>
        /// <returns></returns>
        [NoRemoting]
        public static System.Char GetCharDefault(String AKey, System.Char ADefault, TDataBase ADataBase = null)
        {
            return Convert.ToChar(GetUserDefault(AKey, ADefault.ToString()));
        }

        /// <summary>
        /// get char default
        /// </summary>
        /// <param name="AKey"></param>
        /// <returns>space if key does not exist</returns>
        [NoRemoting]
        public static System.Char GetCharDefault(String AKey)
        {
            return GetCharDefault(AKey, ' ');
        }

        /// <summary>
        /// get double default value
        /// </summary>
        /// <param name="AKey"></param>
        /// <param name="ADefault"></param>
        /// <param name="ADataBase">database object</param>
        /// <returns>0.0 if key does not exist</returns>
        [NoRemoting]
        public static double GetDoubleDefault(String AKey, double ADefault = 0.0, TDataBase ADataBase = null)
        {
            return Convert.ToDouble(GetUserDefault(AKey, ADefault.ToString()));
        }

        /// <summary>
        /// get default int value
        /// </summary>
        /// <param name="AKey">The Key of the User Default that should get retrieved.</param>
        /// <param name="ADefault">The value that should be returned in case the Key is not (yet)
        /// in the User Defaults.
        /// </param>
        /// <param name="ADataBase">database object</param>
        /// <returns>0 if key does not exist</returns>
        [NoRemoting]
        public static System.Int16 GetInt16Default(String AKey, System.Int16 ADefault = 0, TDataBase ADataBase = null)
        {
            return Convert.ToInt16(GetUserDefault(AKey, ADefault.ToString()));
        }

        /// <summary>
        /// get int default
        /// </summary>
        /// <param name="AKey"></param>
        /// <param name="ADefault"></param>
        /// <param name="ADataBase">database object</param>
        /// <returns>0 if key does not exist</returns>
        [NoRemoting]
        public static System.Int32 GetInt32Default(String AKey, System.Int32 ADefault = 0, TDataBase ADataBase = null)
        {
            return Convert.ToInt32(GetUserDefault(AKey, ADefault.ToString()));
        }

        /// <summary>
        /// get int default
        /// </summary>
        /// <param name="AKey"></param>
        /// <param name="ADefault"></param>
        /// <param name="ADataBase">database object</param>
        /// <returns>0 if key does not exist</returns>
        [NoRemoting]
        public static System.Int64 GetInt64Default(String AKey, System.Int64 ADefault = 0, TDataBase ADataBase = null)
        {
            return Convert.ToInt64(GetUserDefault(AKey, ADefault.ToString()));
        }

        /// <summary>
        /// get string default
        /// </summary>
        /// <param name="AKey"></param>
        /// <param name="ADefault"></param>
        /// <param name="ADataBase">database object</param>
        /// <returns>empty string if key does not exist</returns>
        [NoRemoting]
        public static String GetStringDefault(String AKey, String ADefault = "", TDataBase ADataBase = null)
        {
            return GetUserDefault(AKey, ADefault);
        }

        /// <summary>
        /// Reads the User Defaults for a certain user and returns them as a Typed
        /// DataTable.
        /// DB Table: s_user_defaults
        ///
        /// </summary>
        /// <param name="AUserName">User name for which the User Defaults should be read.</param>
        /// <param name="AUserDefaultsDataTable">Typed DataTable that contains the User
        /// Defaults.
        /// </param>
        /// <returns>void</returns>
        [RequireModulePermission("USER")]
        public static void GetUserDefaults(String AUserName, out SUserDefaultsTable AUserDefaultsDataTable)
        {
            LoadUserDefaultsTable(AUserName, out AUserDefaultsDataTable);
        }

        /// <summary>
        /// Gets the value of a UserDefault.
        /// </summary>
        /// <param name="AKey">The Key of the User Default that should get retrieved</param>
        /// <param name="ADefaultValue">The value that should be returned in case there is no value</param>
        /// <param name="ADataBase">database object</param>
        /// <returns>Value of the UserDefault, or the value specified in ADefaultValue if
        /// the UserDefault doesn't exist yet</returns>
        [NoRemoting]
        public static String GetUserDefault(String AKey, String ADefaultValue, TDataBase ADataBase = null)
        {
            String ReturnValue = "";

            TDataBase db = DBAccess.Connect("GetUserDefault", ADataBase);
            TDBTransaction ReadTransaction = new TDBTransaction();

            db.ReadTransaction(ref ReadTransaction,
                delegate
                {
                    if (!SUserDefaultsAccess.Exists(UserInfo.GetUserInfo().UserID, AKey, ReadTransaction))
                    {
                        ReturnValue = ADefaultValue;
                    }
                    else
                    {
                        SUserDefaultsTable DT =
                                SUserDefaultsAccess.LoadByPrimaryKey(UserInfo.GetUserInfo().UserID, AKey, ReadTransaction);
                        ReturnValue = DT[0].DefaultValue;
                    }
                });

            if (ADataBase == null)
            {
                db.CloseDBConnection();
            }

            return ReturnValue;
        }

        /// <summary>
        /// Loads the UserDefaults DataTable from the DB.
        /// DB Table: s_user_defaults
        ///
        /// </summary>
        /// <param name="AUserName">UserID to load the UserDefaults for</param>
        /// <param name="AUserDefaultsDataTable">The loaded UserDefaults DataTable</param>
        /// <param name="ADataBase">An instantiated <see cref="TDataBase" /> object, or null (default = null). If null
        /// gets passed then the Method executes DB commands with a new Database connection</param>
        /// <returns>true if loading of UserDefaults was successful
        /// </returns>
        [NoRemoting]
        public static Boolean LoadUserDefaultsTable(String AUserName,
            out SUserDefaultsTable AUserDefaultsDataTable, TDataBase ADataBase = null)
        {
            Boolean ReturnValue = false;

            TDataBase db = DBAccess.Connect("LoadUserDefaultsTable", ADataBase);
            TDBTransaction ReadTransaction = null;
            bool NewTransaction = false;

            try
            {
                ReadTransaction = db.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                    out NewTransaction);

                if (SUserDefaultsAccess.CountViaSUser(AUserName, ReadTransaction) != 0)
                {
                    AUserDefaultsDataTable = SUserDefaultsAccess.LoadViaSUser(AUserName, null, ReadTransaction,
                        StringHelper.InitStrArr(new string[] { "ORDER BY", SUserDefaultsTable.GetDefaultCodeDBName() }), 0, 0);

                    AUserDefaultsDataTable.AcceptChanges();
                }
                else
                {
                    AUserDefaultsDataTable = new SUserDefaultsTable();
                }

                ReturnValue = true;
            }
            finally
            {
                if (NewTransaction && (ReadTransaction != null))
                {
                    ReadTransaction.Rollback();
                    TLogging.LogAtLevel(9, "TUserDefaults.LoadUserDefaultsTable: rolled back own transaction.");
                }
            }

            if (ADataBase == null)
            {
                db.CloseDBConnection();
            }

            return ReturnValue;
        }

        /// <summary>
        /// Sets a User Default.
        ///
        /// </summary>
        /// <param name="AKey">The Key of the User Default that should get saved.</param>
        /// <param name="AValue">The value that should be stored. Note: This can be anything on
        /// which ToString can be called.</param>
        /// <param name="ASendUpdateInfoToClient">If true, the PetraClient is informed about the
        /// change to the UserDefault (by means of sending a ClientTask)
        /// </param>
        /// <param name="ADataBase">An instantiated <see cref="TDataBase" /> object, or null (default = null). If null
        /// gets passed then the Method executes DB commands with a new Database connection</param>
        /// <returns>void</returns>
        [NoRemoting]
        public static void SetDefault(String AKey, object AValue, Boolean ASendUpdateInfoToClient = true, TDataBase ADataBase = null)
        {
            SUserDefaultsTable UserDefaultsDataTable;

            TDataBase db = DBAccess.Connect("LoadUserDefaultsTable", ADataBase);
            TDBTransaction WriteTransaction = new TDBTransaction();
            bool SubmitOK = false;

            db.WriteTransaction(ref WriteTransaction,
                ref SubmitOK,
                delegate
                {
                    LoadUserDefaultsTable(UserInfo.GetUserInfo().UserID, out UserDefaultsDataTable, db);
                    
                    DataView view = new DataView(UserDefaultsDataTable);
                    view.Sort = SUserDefaultsTable.GetDefaultCodeDBName();
                    int FoundInRow = view.Find(AKey);

                    if (FoundInRow != -1)
                    {
                        // User default found
                        if (AValue.ToString() != view[FoundInRow][SUserDefaultsTable.GetDefaultValueDBName()].ToString())
                        {
                            // Update only if the value is actually different
                            view[FoundInRow][SUserDefaultsTable.GetDefaultValueDBName()] = AValue.ToString();
                            SubmitOK = true;
                        }
                    }
                    else
                    {
                        // User default not found, add it to the user defaults table
                        SUserDefaultsRow row = UserDefaultsDataTable.NewRowTyped();
                        row.UserId = UserInfo.GetUserInfo().UserID;
                        row.DefaultCode = AKey;
                        row.DefaultValue = AValue.ToString();
                        UserDefaultsDataTable.Rows.Add(row);
                        FoundInRow = view.Find(AKey);
                        SubmitOK = true;
                    }

                    if (SubmitOK)
                    {
                        SUserDefaultsAccess.SubmitChanges(UserDefaultsDataTable, WriteTransaction);

                        if (ASendUpdateInfoToClient)
                        {
                            UpdateUserDefaultsOnClient(UserInfo.GetUserInfo().UserID, AKey, AValue.ToString(),
                                view[FoundInRow][SUserDefaultsTable.GetModificationIdDBName()].ToString());
                        }
                    }
                });
        }

        /// <summary>
        /// Update the User Defaults on the Client side (by means of sending a
        /// ClientTask).
        ///
        /// </summary>
        /// <param name="AUserName">User name for which the User Defaults should be updated.</param>
        /// <param name="AChangedUserDefaultCode">DefaultCode(s) that has/have changed. Note:
        /// several DefaultCodes can be concatenated using the
        /// GCLIENTTASKPARAMETER_SEPARATOR constant!</param>
        /// <param name="AChangedUserDefaultValue">DefaultValue(s) that has/have changed.
        /// Note: several DefaultValues can be concatenated using the
        /// GCLIENTTASKPARAMETER_SEPARATOR constant!</param>
        /// <param name="AChangedUserDefaultModId">ModificationID(s) that has/have changed.
        /// Note: several ModificationIDs can be concatenated using the
        /// GCLIENTTASKPARAMETER_SEPARATOR constant!</param>
        /// <param name="ASingleCode">Set to true if the situation from which this function is
        /// called arose because a single DefaultCode has changed, or to false to
        /// indicate that the situation from which this function is called arose
        /// because (potentially) several DefaultCodes have changed.
        /// </param>
        /// <returns>void</returns>
        [NoRemoting]
        public static void UpdateUserDefaultsOnClient(String AUserName,
            String AChangedUserDefaultCode,
            String AChangedUserDefaultValue,
            String AChangedUserDefaultModId,
            Boolean ASingleCode = true)
        {
            String SingleOrMultipleIndicator;

            TLogging.LogAtLevel(7, "TMaintenanceUserDefaults.UpdateUserDefaultsOnClient: calling DomainManager.ClientTaskAdd...");

            if (AChangedUserDefaultCode == null)
            {
                if (AUserName == UserInfo.GetUserInfo().UserID)
                {
                    // Queue a ClientTask to the current User's PetraClient
                    DomainManager.CurrentClient.FTasksManager.ClientTaskAdd(
                        SharedConstants.CLIENTTASKGROUP_USERDEFAULTSREFRESH, "All",
                        null, null, null, null, 1);
                }
                else
                {
                    // Queue a ClientTask to any but the current User's PetraClient
                    TClientManager.QueueClientTask(AUserName,
                        SharedConstants.CLIENTTASKGROUP_USERDEFAULTSREFRESH,
                        "All",
                        null, null, null, null,
                        1, DomainManager.GClientID);
                }
            }
            else
            {
                if (ASingleCode)
                {
                    SingleOrMultipleIndicator = "Single";
                }
                else
                {
                    SingleOrMultipleIndicator = "Multiple";
                }

                if (AUserName == UserInfo.GetUserInfo().UserID)
                {
                    // Queue a ClientTask to the current User's PetraClient
                    if (DomainManager.CurrentClient != null)
                    {
                        DomainManager.CurrentClient.FTasksManager.ClientTaskAdd(SharedConstants.CLIENTTASKGROUP_USERDEFAULTSREFRESH,
                            SingleOrMultipleIndicator,
                            AChangedUserDefaultCode,
                            AChangedUserDefaultValue,
                            AChangedUserDefaultModId,
                            UserInfo.GetUserInfo().ProcessID,
                            1);
                    }
                }

                // Send the same ClientTask to all other running PetraClient instances where
                // the same user is logged in!
                TClientManager.QueueClientTask(AUserName,
                    SharedConstants.CLIENTTASKGROUP_USERDEFAULTSREFRESH,
                    SingleOrMultipleIndicator,
                    AChangedUserDefaultCode,
                    AChangedUserDefaultValue,
                    AChangedUserDefaultModId,
                    null,
                    1,
                    DomainManager.GClientID);
            }
        }

        /// <summary>
        /// Update the User Defaults on the Client side (by means of sending a
        /// ClientTask).
        ///
        /// </summary>
        /// <param name="AUserName">User name for which the User Defaults should be updated.</param>
        /// <param name="AChangedUserDefaultsDT">DataTable containing one ore more UserDefaults
        /// that have changed and that should be updated on the Client side
        /// </param>
        /// <returns>void</returns>
        [NoRemoting]
        public static void UpdateUserDefaultsOnClient(String AUserName, SUserDefaultsTable AChangedUserDefaultsDT)
        {
            String ChangedUserDefaultCodes;
            String ChangedUserDefaultValues;
            String ChangedUserDefaultModIds;
            Int16 Counter;

            ChangedUserDefaultCodes = "";
            ChangedUserDefaultValues = "";
            ChangedUserDefaultModIds = "";

            if (AChangedUserDefaultsDT != null)
            {
                for (Counter = 0; Counter <= AChangedUserDefaultsDT.Rows.Count - 1; Counter += 1)
                {
                    ChangedUserDefaultCodes = ChangedUserDefaultCodes + RemotingConstants.GCLIENTTASKPARAMETER_SEPARATOR +
                                              AChangedUserDefaultsDT[Counter].DefaultCode;
                    ChangedUserDefaultValues = ChangedUserDefaultValues + RemotingConstants.GCLIENTTASKPARAMETER_SEPARATOR +
                                               AChangedUserDefaultsDT[Counter].DefaultValue;
                    ChangedUserDefaultModIds = ChangedUserDefaultModIds + RemotingConstants.GCLIENTTASKPARAMETER_SEPARATOR +
                                               AChangedUserDefaultsDT[Counter].ModificationId;
                }

                if (ChangedUserDefaultCodes != "")
                {
                    // Strip off leading GCLIENTTASKPARAMETER_SEPARATOR
                    ChangedUserDefaultCodes = ChangedUserDefaultCodes.Substring(1, ChangedUserDefaultCodes.Length - 1);
                    ChangedUserDefaultValues = ChangedUserDefaultValues.Substring(1, ChangedUserDefaultValues.Length - 1);
                    ChangedUserDefaultModIds = ChangedUserDefaultModIds.Substring(1, ChangedUserDefaultModIds.Length - 1);
                    UpdateUserDefaultsOnClient(AUserName, ChangedUserDefaultCodes, ChangedUserDefaultValues, ChangedUserDefaultModIds, false);
                }
            }
        }
    }
}
