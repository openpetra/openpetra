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
using System.Data.Odbc;
using System.Threading;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MSysMan.Data.Access;
using Ict.Common.Data;
using Ict.Petra.Shared;

namespace Ict.Petra.Server.MSysMan.Maintenance
{
    /// <summary>
    /// Reads and saves a DataTable for the User Defaults.
    ///
    /// </summary>
    public class TUserDefaults
    {
        private const String USERDEFAULTSDT_NAME = "UserDefaultsCacheDT";

        /// <summary>internal cache DataSet of the UserDefaults  for current user only</summary>
        private static DataSet UUserDefaultsDS;

        /// <summary>internal cache DataTable of the UserDefaults  for current user only</summary>
        private static SUserDefaultsTable UUserDefaultsDT;

        /// <summary>DataView on the internal cache DataTable</summary>
        private static DataView UUserDefaultsDV;

        /// <summary>used to control read and write access to the cache</summary>
        private static System.Threading.ReaderWriterLock UReadWriteLock;

        /// <summary>tells whether the cache is containing data, or not</summary>
        private static Boolean UTableCached;

        /// <summary>time when this object was instantiated</summary>
        private DateTime FStartTime;

        /// <summary>
        /// initialize some static variables
        /// </summary>
        public static void InitializeUnit()
        {
            UReadWriteLock = new System.Threading.ReaderWriterLock();
            UTableCached = false;
            UUserDefaultsDS = new DataSet();
            UUserDefaultsDS.Tables.Add(new SUserDefaultsTable(USERDEFAULTSDT_NAME));
            UUserDefaultsDT = (SUserDefaultsTable)UUserDefaultsDS.Tables[USERDEFAULTSDT_NAME];
            UUserDefaultsDV = new DataView(UUserDefaultsDT);
            UUserDefaultsDV.Sort = SUserDefaultsTable.GetDefaultCodeDBName();
        }

        /// <summary>
        /// constructor
        /// </summary>
        public TUserDefaults() : base()
        {
            // $IFDEF DEBUGMODE if TSrvSetting.DL >= 9 then Console.WriteLine(this.GetType.FullName + ' created: Instance hash is ' + this.GetHashCode().ToString()); $ENDIF
            FStartTime = DateTime.Now;
        }

        /// <summary>
        /// Find out if a user default exists already.
        /// This is required where the default should be calculated otherwise
        /// (e.g. FINANCE_REPORTING_SHOWDIFFFINANCIALYEARSELECTION)
        /// </summary>
        /// <returns>true if a default with the given key already exists
        /// </returns>
        public static bool HasDefault(String AKey)
        {
            return TInternal.HasUserDefault(AKey);
        }

        /// <summary>
        /// get boolean default
        /// </summary>
        /// <param name="AKey"></param>
        /// <param name="ADefault"></param>
        /// <returns></returns>
        public static bool GetBooleanDefault(String AKey, bool ADefault)
        {
            return Convert.ToBoolean(TInternal.GetUserDefault(AKey, ADefault.ToString()));
        }

        /// <summary>
        /// get boolean default
        /// </summary>
        /// <param name="AKey"></param>
        /// <returns>true if key does not exist</returns>
        public static bool GetBooleanDefault(String AKey)
        {
            return GetBooleanDefault(AKey, true);
        }

        /// <summary>
        /// get char default
        /// </summary>
        /// <param name="AKey"></param>
        /// <param name="ADefault"></param>
        /// <returns></returns>
        public static System.Char GetCharDefault(String AKey, System.Char ADefault)
        {
            return Convert.ToChar(TInternal.GetUserDefault(AKey, ADefault.ToString()));
        }

        /// <summary>
        /// get char default
        /// </summary>
        /// <param name="AKey"></param>
        /// <returns>space if key does not exist</returns>
        public static System.Char GetCharDefault(String AKey)
        {
            return GetCharDefault(AKey, ' ');
        }

        /// <summary>
        /// get double default value
        /// </summary>
        /// <param name="AKey"></param>
        /// <param name="ADefault"></param>
        /// <returns></returns>
        public static double GetDoubleDefault(String AKey, double ADefault)
        {
            return Convert.ToDouble(TInternal.GetUserDefault(AKey, ADefault.ToString()));
        }

        /// <summary>
        /// get double default
        /// </summary>
        /// <param name="AKey"></param>
        /// <returns>0.0 if key does not exist</returns>
        public static double GetDoubleDefault(String AKey)
        {
            return GetDoubleDefault(AKey, 0.0);
        }

        /// <summary>
        /// get default int value
        /// </summary>
        /// <param name="AKey">The Key of the User Default that should get retrieved.</param>
        /// <param name="ADefault">The value that should be returned in case the Key is not (yet)
        /// in the User Defaults.
        /// </param>
        /// <returns>void</returns>
        public static System.Int16 GetInt16Default(String AKey, System.Int16 ADefault)
        {
            return Convert.ToInt16(TInternal.GetUserDefault(AKey, ADefault.ToString()));
        }

        /// <summary>
        /// get int default value
        /// </summary>
        /// <param name="AKey"></param>
        /// <returns>0 if key does not exist</returns>
        public static System.Int16 GetInt16Default(String AKey)
        {
            return GetInt16Default(AKey, 0);
        }

        /// <summary>
        /// get int default
        /// </summary>
        /// <param name="AKey"></param>
        /// <param name="ADefault"></param>
        /// <returns></returns>
        public static System.Int32 GetInt32Default(String AKey, System.Int32 ADefault)
        {
            return Convert.ToInt32(TInternal.GetUserDefault(AKey, ADefault.ToString()));
        }

        /// <summary>
        /// get int default
        /// </summary>
        /// <param name="AKey"></param>
        /// <returns>0 if key does not exist</returns>
        public static System.Int32 GetInt32Default(String AKey)
        {
            return GetInt32Default(AKey, 0);
        }

        /// <summary>
        /// get int default
        /// </summary>
        /// <param name="AKey"></param>
        /// <param name="ADefault"></param>
        /// <returns></returns>
        public static System.Int64 GetInt64Default(String AKey, System.Int64 ADefault)
        {
            return Convert.ToInt64(TInternal.GetUserDefault(AKey, ADefault.ToString()));
        }

        /// <summary>
        /// get int default
        /// </summary>
        /// <param name="AKey"></param>
        /// <returns>0 if key does not exist</returns>
        public static System.Int64 GetInt64Default(String AKey)
        {
            return GetInt64Default(AKey, 0);
        }

        /// <summary>
        /// get string default
        /// </summary>
        /// <param name="AKey"></param>
        /// <param name="ADefault"></param>
        /// <returns></returns>
        public static String GetStringDefault(String AKey, String ADefault)
        {
            return TInternal.GetUserDefault(AKey, ADefault);
        }

        /// <summary>
        /// get string default
        /// </summary>
        /// <param name="AKey"></param>
        /// <returns>empty string if key does not exist</returns>
        public static String GetStringDefault(String AKey)
        {
            return GetStringDefault(AKey, "");
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
        public static void GetUserDefaults(String AUserName, out SUserDefaultsTable AUserDefaultsDataTable)
        {
#if DEBUGMODE
            if (TSrvSetting.DL >= 7)
            {
                Console.WriteLine("TMaintenanceUserDefaults.GetUserDefaults called.");
            }
#endif

            if (UUserDefaultsDT == null)
            {
                // Initialisation needs to be done once!
                InitializeUnit();
            }

            if (((AUserName == UserInfo.GUserInfo.UserID) && ((!UTableCached))) || (AUserName != UserInfo.GUserInfo.UserID))
            {
                LoadUserDefaultsTable(AUserName, true, out AUserDefaultsDataTable);
                UTableCached = true;
            }

            try
            {
                try
                {
#if DEBUGMODE
                    if (TSrvSetting.DL >= 7)
                    {
                        Console.WriteLine("TMaintenanceUserDefaults.GetUserDefaults waiting for a ReaderLock...");
                    }
#endif

                    /* Try to get a read lock on the cache table [We don't specify a timeout because (1) reading an emptied cache would lead to problems (it is emptied before the DB queries are issued), (2) reading the DB tables into the cached table
                     *should be fairly quick] */
                    UReadWriteLock.AcquireReaderLock(SharedConstants.THREADING_WAIT_INFINITE);
#if DEBUGMODE
                    if (TSrvSetting.DL >= 7)
                    {
                        Console.WriteLine("TMaintenanceUserDefaults.GetUserDefaults grabbed a ReaderLock.");
                    }
#endif
                    AUserDefaultsDataTable = UUserDefaultsDT;
                }
                finally
                {
                    // Release read lock on the cache table
                    UReadWriteLock.ReleaseReaderLock();
#if DEBUGMODE
                    if (TSrvSetting.DL >= 7)
                    {
                        Console.WriteLine("TMaintenanceUserDefaults.GetUserDefaults released the ReaderLock.");
                    }
#endif
                }
            }
#if DEBUGMODE
            catch (Exception exp)
            {
                if (TSrvSetting.DL >= 7)
                {
                    Console.WriteLine("Exception in TMaintenanceUserDefaults.GetUserDefaults: " + exp.ToString());
                }

                throw;
            }
#else
            catch (Exception)
            {
                throw;
            }
#endif
        }

        /// <summary>
        /// Loads the UserDefaults DataTable from the DB.
        /// DB Table: s_user_defaults
        ///
        /// </summary>
        /// <param name="AUserName">UserID to load the UserDefaults for</param>
        /// <param name="AMergeChangesToServerSideCache">Set to true if the UserDefaults were
        /// (re)loaded for the current user and the internal cache needs to be updated</param>
        /// <param name="AUserDefaultsDataTable">The loaded UserDefaults DataTable</param>
        /// <returns>true if loading of UserDefaults was successful
        /// </returns>
        public static Boolean LoadUserDefaultsTable(String AUserName,
            Boolean AMergeChangesToServerSideCache,
            out SUserDefaultsTable AUserDefaultsDataTable)
        {
            Boolean ReturnValue;
            TDBTransaction ReadTransaction;
            Boolean NewTransaction = false;
            Boolean ReaderLockWasHeld;
            Boolean WriteLockTakenOut;
            LockCookie UpgradeLockCookie = new LockCookie();

#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine("TMaintenanceUserDefaults.LoadUserDefaultsTable called in the AppDomain " + Thread.GetDomain().FriendlyName + '.');
            }
#endif
            WriteLockTakenOut = false;
            ReaderLockWasHeld = false;
            try
            {
                try
                {
                    try
                    {
                        ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                            TEnforceIsolationLevel.eilMinimum,
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
                    }
                    finally
                    {
                        if (NewTransaction)
                        {
                            DBAccess.GDBAccessObj.CommitTransaction();
#if DEBUGMODE
                            if (TSrvSetting.DL >= 9)
                            {
                                Console.WriteLine("TMaintenanceUserDefaults.LoadUserDefaultsTable: committed own transaction.");
                            }
#endif
                        }
                    }

                    if ((AUserName == UserInfo.GUserInfo.UserID) && (AMergeChangesToServerSideCache))
                    {
                        /*
                         * The UserDefaults were (re)loaded for the current user --> update
                         * internal cache (modified UserDefaults in the cache will be replaced
                         * with values from the DB, deleted UserDefaults in the cache will be
                         * recreated from the DB, added UserDefaults in the cache will be
                         * unaffected).
                         */
                        MergeChanges(UUserDefaultsDT, AUserDefaultsDataTable);
                        UUserDefaultsDT.AcceptChanges();
                    }

                    ReturnValue = true;
                }
                finally
                {
                    if (WriteLockTakenOut)
                    {
                        if (!ReaderLockWasHeld)
                        {
                            // Other threads are now free to obtain a read lock on the cache table.
                            UReadWriteLock.ReleaseWriterLock();
#if DEBUGMODE
                            if (TSrvSetting.DL >= 7)
                            {
                                Console.WriteLine("TMaintenanceUserDefaults.LoadUserDefaultsTable released the WriterLock.");
                            }
#endif
                        }
                        else
                        {
#if DEBUGMODE
                            if (TSrvSetting.DL >= 7)
                            {
                                Console.WriteLine("TMaintenanceUserDefaults.ReloadUserDefaults waiting for downgrading to a ReaderLock...");
                            }
#endif

                            // Downgrade from a WriterLock to a ReaderLock again!
                            UReadWriteLock.DowngradeFromWriterLock(ref UpgradeLockCookie);
#if DEBUGMODE
                            if (TSrvSetting.DL >= 7)
                            {
                                Console.WriteLine("TMaintenanceUserDefaults.ReloadUserDefaults downgraded to a ReaderLock.");
                            }
#endif
                        }
                    }
                }
            }
#if DEBUGMODE
            catch (Exception exp)
            {
                Console.WriteLine("TMaintenanceUserDefaults.LoadUserDefaultsTable: Error loading UserDefaults! " + "Possible cause: " + exp.ToString());
                throw;
            }
#else
            catch (Exception)
            {
                throw;
            }
#endif
            return ReturnValue;
        }

        /// <summary>
        /// Merges changes to the UserDefaults DataTable from one copy of it to another
        /// copy of it.
        ///
        /// @comment This function takes a similar approach as procedure
        /// 'CopyModificationIDsOver' in Ict.Common.Data.Utilities, but has some
        /// important differences.
        ///
        /// </summary>
        /// <param name="ADestinationDT">UserDefaults DataTable that will be merged to</param>
        /// <param name="ASourceDT">UserDefaults DataTable that is the source of the merge
        /// </param>
        /// <returns>void</returns>
        public static void MergeChanges(SUserDefaultsTable ADestinationDT, SUserDefaultsTable ASourceDT)
        {
            Int16 Counter;

            DataColumn[] PrimaryKeyColumns;
            object[] PrimaryKeyObj;
            SUserDefaultsRow UpdateRow;
            SUserDefaultsRow AddedRow;

            if (ADestinationDT == null)
            {
                throw new System.ArgumentException("ADestinationDT must not be nil");
            }

            if (ASourceDT == null)
            {
                throw new System.ArgumentException("ASourceDT must not be nil");
            }

            PrimaryKeyColumns = ASourceDT.PrimaryKey;
            PrimaryKeyObj = new object[PrimaryKeyColumns.Length];

            for (Counter = 0; Counter <= ASourceDT.Rows.Count - 1; Counter += 1)
            {
                // TLogging.Log('MergeChanges: working on Row #' + Counter.ToString + ' (DefaultCode: ''' + ASourceDT[Counter].DefaultCode + '''');
                // for Counter2 := 0 to Length(PrimaryKeyColumns)  1 do
                // begin
                // PrimaryKeyObj[Counter2] := ASourceDT.Rows[Counter].Item[PrimaryKeyColumns[Counter2]] as TObject;
                // TLogging.Log('CopyModificationIDsOver: working on Row #' + Counter.ToString + '.  PrimaryKeyObj[' + Counter2.ToString + ']: ' + PrimaryKeyObj[Counter2].ToString);
                // end;
                UpdateRow = (SUserDefaultsRow)ADestinationDT.Rows.Find(new object[] { ASourceDT[Counter].UserId, ASourceDT[Counter].DefaultCode });

                if (UpdateRow != null)
                {
                    // avoid checking for DefaultValue if it is NULL
                    // see bug http:bugs.om.org/petra/show_bug.cgi?id=741
                    if ((ASourceDT[Counter].IsDefaultValueNull() != ((SUserDefaultsRow)UpdateRow).IsDefaultValueNull())
                        || ASourceDT[Counter].IsDefaultValueNull() || (ASourceDT[Counter].DefaultValue != ((SUserDefaultsRow)UpdateRow).DefaultValue))
                    {
                        // TLogging.Log('MergeChanges: copying Data over for Row #' + Counter.ToString + ' (DefaultCode: ''' + ASourceDT[Counter].DefaultCode + '''');
                        ADestinationDT.Rows.Remove(UpdateRow);
                        AddedRow = ADestinationDT.NewRowTyped(false);
                        AddedRow.ItemArray = ASourceDT[Counter].ItemArray;
                        ADestinationDT.Rows.Add(AddedRow);

                        // Mark row as no longer being new
                        AddedRow.AcceptChanges();

                        // Mark row as beeing changed
                        AddedRow.DefaultValue = AddedRow.DefaultValue + ' ';
                        AddedRow.AcceptChanges();
                        AddedRow.DefaultValue = AddedRow.DefaultValue.Substring(0, AddedRow.DefaultValue.Length - 1);
                    }
                }
                else
                {
                    // TLogging.Log('MergeChanges: Row #' + Counter.ToString + ': no matching Row in ADestinationDT found > creating new one!');
                    AddedRow = ADestinationDT.NewRowTyped(false);
                    AddedRow.ItemArray = ASourceDT[Counter].ItemArray;
                    ADestinationDT.Rows.Add(AddedRow);
                }
            }
        }

        /// <summary>
        /// Reloads the User Defaults for the specified user from the database.
        /// DB Table: s_user_defaults
        ///
        /// </summary>
        /// <param name="AUserName">User name for which the User Defaults should be read.</param>
        /// <param name="AMergeChangesToServerSideCache">Set to true if the UserDefaults were
        /// (re)loaded for the current user and the internal cache needs to be updated</param>
        /// <param name="AUserDefaultsDataTable">Typed DataTable that contains the User
        /// Defaults.
        /// </param>
        /// <returns>void</returns>
        public static void ReloadUserDefaults(String AUserName, Boolean AMergeChangesToServerSideCache, out SUserDefaultsTable AUserDefaultsDataTable)
        {
            LoadUserDefaultsTable(AUserName, AMergeChangesToServerSideCache, out AUserDefaultsDataTable);
        }

        /// <summary>
        /// Saves the User Defaults for the specified user.
        /// This method is designed to get called from the Client side.
        /// DB Table: s_user_defaults
        ///
        /// </summary>
        /// <param name="AUserName">User name for which the User Defaults should be saved.</param>
        /// <param name="AUserDefaultsDataTable">The Typed DataTable that contains changed
        /// and/or added User Defaults.</param>
        /// <param name="AVerificationResult">Nil if DB calls succeded, otherwise filled with
        /// 1..n TVerificationResult objects (contain DB call exceptions).</param>
        /// <returns>true if the User Defaults could be saved successfully or if there
        /// were no DataRows in the AUserDefaultsDataTable. false indicates that a DB
        /// call resulted in an error. Inspect AVerificationResult to retrieve the
        /// error.
        /// </returns>
        public static Boolean SaveUserDefaultsFromClientSide(String AUserName,
            ref SUserDefaultsTable AUserDefaultsDataTable,
            out TVerificationResultCollection AVerificationResult)
        {
            Boolean SubmissionOK;
            Int16 Counter;
            SUserDefaultsRow UserDefaultServerSideRow;

            AVerificationResult = null;
            SubmissionOK = true;

            if ((AUserDefaultsDataTable != null) && (AUserDefaultsDataTable.Rows.Count > 0))
            {
#if DEBUGMODE
                if (TSrvSetting.DL >= 8)
                {
                    Console.WriteLine(
                        "TMaintenanceUserDefaults.SaveUserDefaultsFromClientSide: Saving " + (AUserDefaultsDataTable.Rows.Count).ToString() +
                        " User Defaults...");
                }
#endif
#if DEBUGMODE
                if (TSrvSetting.DL >= 8)
                {
                    Console.WriteLine(
                        "TMaintenanceUserDefaults.SaveUserDefaultsFromClientSide: Row[0] --- UserId: " + AUserDefaultsDataTable[0].UserId +
                        "; DefaultCode: " + AUserDefaultsDataTable[0].DefaultCode + "; DefaultValue: '" + AUserDefaultsDataTable[0].DefaultValue +
                        "'");
                }
#endif

                if (AUserName == UserInfo.GUserInfo.UserID)
                {
                    // Update the serverside copy of the UserDefaults with what was submitted
                    // from the Client.
#if DEBUGMODE
                    if (TSrvSetting.DL >= 7)
                    {
                        Console.WriteLine("TMaintenanceUserDefaults.SaveUserDefaultsFromClientSide waiting for a WriterLock...");
                    }
#endif

                    // Prevent other threads from obtaining a read lock on the cache table while we are merging the cache table!
                    UReadWriteLock.AcquireWriterLock(SharedConstants.THREADING_WAIT_INFINITE);
#if DEBUGMODE
                    if (TSrvSetting.DL >= 7)
                    {
                        Console.WriteLine("TMaintenanceUserDefaults.SaveUserDefaultsFromClientSide grabbed a WriterLock...");
                    }
#endif
                    try
                    {
                        try
                        {
                            /*
                             * The UserDefaults were submitted from the Client side for the current
                             * user --> update internal cache (modified UserDefaults in the cache
                             * will be replaced with values from the Client side, deleted
                             * UserDefaults in the cache will be recreated from the Client side,
                             * added UserDefaults in the cache will be unaffected).
                             */

                            // $IFDEF DEBUGMODE if TSrvSetting.DL >= 7 then Console.WriteLine('Before merge: UUserDefaultsDS.Tables.Count: ' + UUserDefaultsDS.Tables.Count.ToString); $ENDIF
                            // $IFDEF DEBUGMODE if TSrvSetting.DL >= 7 then Console.WriteLine('Before merge: UUserDefaultsDS.Tables[0].Rows.Count: ' + UUserDefaultsDS.Tables[0].Rows.Count.ToString); $ENDIF
#if DEBUGMODE
                            if (TSrvSetting.DL >= 7)
                            {
                                Console.WriteLine("Before merge: UUserDefaultsDT.Rows.Count: " + UUserDefaultsDT.Rows.Count.ToString());
                            }
#endif
#if DEBUGMODE
                            if (TSrvSetting.DL >= 7)
                            {
                                DataTable TmpChangesDT = UUserDefaultsDT.GetChangesTyped();

                                if (TmpChangesDT != null)
                                {
                                    Console.WriteLine("Before merge: UUserDefaultsDT Changed Rows: " + TmpChangesDT.Rows.Count.ToString());
                                }
                            }
#endif
                            MergeChanges(UUserDefaultsDT, AUserDefaultsDataTable);

                            // $IFDEF DEBUGMODE if TSrvSetting.DL >= 7 then Console.WriteLine('After merge: UUserDefaultsDS.Tables.Count: ' + UUserDefaultsDS.Tables.Count.ToString); $ENDIF
                            // $IFDEF DEBUGMODE if TSrvSetting.DL >= 7 then Console.WriteLine('After merge: UUserDefaultsDS.Tables[0].Rows.Count: ' + UUserDefaultsDS.Tables[0].Rows.Count.ToString); $ENDIF
#if DEBUGMODE
                            if (TSrvSetting.DL >= 7)
                            {
                                Console.WriteLine("After merge: UUserDefaultsDT.Rows.Count: " + UUserDefaultsDT.Rows.Count.ToString());
                            }
#endif
#if DEBUGMODE
                            if (TSrvSetting.DL >= 7)
                            {
                                DataTable TmpChangesDT = UUserDefaultsDT.GetChangesTyped();

                                if (TmpChangesDT != null)
                                {
                                    Console.WriteLine("After merge: UUserDefaultsDT Changed Rows: " + TmpChangesDT.Rows.Count.ToString());
                                }
                            }
#endif
#if DEBUGMODE
                            int TmpRow = UUserDefaultsDV.Find("MailroomLastPerson");

                            if (TmpRow != -1)
                            {
                                if (TSrvSetting.DL >= 7)
                                {
                                    Console.WriteLine("MailroomLastPerson value: '" +
                                        UUserDefaultsDV[TmpRow][SUserDefaultsTable.GetDefaultValueDBName()].ToString() + "'");
                                }
                            }
#endif

                            // User Defaults were submitted from the Client side for any user but
                            // the current user > just save the (now updated) serverside copy
                            // of the UserDefaults.
#if DEBUGMODE
                            if (TSrvSetting.DL >= 7)
                            {
                                Console.WriteLine(
                                    "TMaintenanceUserDefaults.SaveUserDefaultsFromClientSide: merged changed data from the Client side into the Server-side UserDefaults cache; saving the Server-side UserDefaults cache now.");
                            }
#endif
                            SubmissionOK = SaveUserDefaultsFromServerSide(out AVerificationResult, false);

                            if (SubmissionOK)
                            {
                                /*
                                 * Take over changed ModificationIDs (they get changed when existing
                                 * UserDefaults are saved. They need to be communicated back to the Client
                                 * to allow the Client to update a previously changed UserDefault again!).
                                 * This is done by taking them over from the server-side DT into the DT
                                 * that got passed in from the Client.
                                 */
                                for (Counter = 0; Counter <= AUserDefaultsDataTable.Rows.Count - 1; Counter += 1)
                                {
                                    UserDefaultServerSideRow =
                                        (SUserDefaultsRow)UUserDefaultsDT.Rows.Find(new object[] { AUserDefaultsDataTable[Counter].UserId,
                                                                                                   AUserDefaultsDataTable[Counter].DefaultCode });

                                    if (UserDefaultServerSideRow != null)
                                    {
                                        if (!AUserDefaultsDataTable[Counter].IsModificationIdNull())
                                        {
                                            AUserDefaultsDataTable[Counter].ModificationId = UserDefaultServerSideRow.ModificationId;
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception Exp)
                        {
                            TLogging.Log("Exception occured in TMaintenanceUserDefaults.SaveUserDefaultsFromClientSide: " + Exp.ToString());
                            throw;
                        }
                    }
                    finally
                    {
                        // Other threads are now free to obtain a read lock on the cache table.
                        UReadWriteLock.ReleaseWriterLock();
#if DEBUGMODE
                        if (TSrvSetting.DL >= 7)
                        {
                            Console.WriteLine("TMaintenanceUserDefaults.SaveUserDefaultsFromClientSide released the WriterLock.");
                        }
#endif
                    }
                }
                else
                {
                    // User Defaults were submitted from the Client side for any user but the current user
                    SubmissionOK = TUserDefaults.SaveUserDefaultsTable(AUserName,
                        ref AUserDefaultsDataTable,
                        null,
                        ref AVerificationResult);
                }
            }
            else
            {
            }

            return SubmissionOK;
        }

        /// <summary>
        /// Saves User Defaults. The UserDefaults can be saved for any user - as
        /// specified in the s_user_id_c column.
        /// DB Table: s_user_defaults
        ///
        /// @comment This function might determine the need for informing the
        /// PetraClient about the change to the UserDefault even if
        /// 'AMergeChangesToServerSideCache' is false: that would be the case if an
        /// EDBConcurrencyException occurs!
        ///
        /// </summary>
        /// <param name="AUserName">UserID to save the UserDefaults for</param>
        /// <param name="AUserDefaultsDataTable">The Typed DataTable that contains changed
        /// and/or added User Defaults for a User.</param>
        /// <param name="AWriteTransaction">Current Odbc Transaction</param>
        /// <param name="AVerificationResult">Nil if DB calls succeded, otherwise filled with
        /// 1..n TVerificationResult objects (contain DB call exceptions).</param>
        /// <param name="ASendUpdateInfoToClient">If true, the PetraClient is informed about the
        /// change to the UserDefault (by means of sending a ClientTask)</param>
        /// <returns>true if the User Defaults could be saved successfully or if there
        /// were no DataRows in the AUserDefaultsDataTable. false indicates that a DB
        /// call resulted in an error. Inspect AVerificationResult to retrieve the
        /// error.
        /// </returns>
        public static Boolean SaveUserDefaultsTable(String AUserName,
            ref SUserDefaultsTable AUserDefaultsDataTable,
            TDBTransaction AWriteTransaction,
            ref TVerificationResultCollection AVerificationResult,
            Boolean ASendUpdateInfoToClient)
        {
            Boolean ReturnValue;
            Boolean NewTransaction = false;
            Boolean SubmissionOK = false;
            TDBTransaction WriteTransaction;
            Int32 SavingAttempts = 0;
            SUserDefaultsTable ChangedUserDefaultsDT;
            SUserDefaultsTable ChangedUserDefaults2DT;
            SUserDefaultsTable RefreshedUserDefaultsDataTable = null;

            if ((AUserDefaultsDataTable != null) && (AUserDefaultsDataTable.Rows.Count > 0))
            {
                ChangedUserDefaultsDT = AUserDefaultsDataTable.GetChangesTyped();

                if ((ChangedUserDefaultsDT != null) && (ChangedUserDefaultsDT.Rows.Count > 0))
                {
#if DEBUGMODE
                    if (TSrvSetting.DL >= 8)
                    {
                        Console.WriteLine(
                            "TMaintenanceUserDefaults.SaveUserDefaultsTable: Saving " + (ChangedUserDefaultsDT.Rows.Count).ToString() +
                            " User Defaults...");
                    }
#endif
#if DEBUGMODE
                    if (TSrvSetting.DL >= 8)
                    {
                        Console.WriteLine(
                            "TMaintenanceUserDefaults.SaveUserDefaultsTable: Row[0] --- UserId: " + ChangedUserDefaultsDT[0].UserId +
                            "; DefaultCode: " +
                            ChangedUserDefaultsDT[0].DefaultCode + "; DefaultValue: " + ChangedUserDefaultsDT[0].DefaultValue);
                    }
#endif

                    if (AWriteTransaction == null)
                    {
                        WriteTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                            TEnforceIsolationLevel.eilMinimum,
                            out NewTransaction);
                    }
                    else
                    {
                        WriteTransaction = AWriteTransaction;
                    }

                    try
                    {
                        do
                        {
                            try
                            {
                                SubmissionOK = SUserDefaultsAccess.SubmitChanges(AUserDefaultsDataTable, WriteTransaction, out AVerificationResult);
                                SavingAttempts = SavingAttempts + 1;
                            }
                            catch (EDBConcurrencyException)
                            {
#if DEBUGMODE
                                if (TSrvSetting.DL >= 8)
                                {
                                    TLogging.Log(
                                        "TMaintenanceUserDefaults.SaveUserDefaultsTable: EDBConcurrencyException occured --> refreshing cached UserDefaults with UserDefaults from the DB!");
                                }
#endif

                                // The same user has saved the same UserDefault after we have read it in
                                // (this can only happen if the same user is logged in twice).
                                // > Read in the UserDefaults for this user again and merge them
                                // into the ones that are to be submitted, notify the Client to
                                // reload the UserDefaults and start submitting again!
                                ReloadUserDefaults(UserInfo.GUserInfo.UserID, false, out RefreshedUserDefaultsDataTable);
                                DataUtilities.CopyModificationIDsOver(ChangedUserDefaultsDT, RefreshedUserDefaultsDataTable);
                                DataUtilities.CopyModificationIDsOver(AUserDefaultsDataTable, ChangedUserDefaultsDT);
                                SavingAttempts = SavingAttempts + 1;
                                ASendUpdateInfoToClient = true;
                            }
#if DEBUGMODE
                            catch (Exception Exp)
                            {
                                if (TSrvSetting.DL >= 8)
                                {
                                    TLogging.Log("TMaintenanceUserDefaults.SaveUserDefaultsTable: Exception occured: " + Exp.ToString());
                                }

                                throw;
                            }
#endif
                        } while (!((SavingAttempts > 1) || SubmissionOK));

#if DEBUGMODE
                        if (TSrvSetting.DL >= 8)
                        {
                            Console.WriteLine("TMaintenanceUserDefaults.SaveUserDefaultsTable: after saving.");
                        }
#endif
#if DEBUGMODE
                        if (TSrvSetting.DL >= 8)
                        {
                            Console.WriteLine(
                                "TMaintenanceUserDefaults.SaveUserDefaultsTable: ChangedUserDefaultsDT.Rows.Count: " +
                                ChangedUserDefaultsDT.Rows.Count.ToString());
                        }
#endif

                        if (SubmissionOK)
                        {
                            ChangedUserDefaults2DT = AUserDefaultsDataTable.GetChangesTyped();

                            // Tell the Client the updated UserDefaults (needed for the ModificationIDs)
                            if (ASendUpdateInfoToClient)
                            {
#if DEBUGMODE
                                if (TSrvSetting.DL >= 8)
                                {
                                    Console.WriteLine("TMaintenanceUserDefaults.SaveUserDefaultsTable: ASendUpdateInfoToClient = true");
                                }
#endif
                                UpdateUserDefaultsOnClient(AUserName, ChangedUserDefaults2DT);
                            }
                            else
                            {
                                foreach (DataRow ChangedUDDR in ChangedUserDefaults2DT.Rows)
                                {
                                    // If a new UserDefault has been INSERTed into the DB Table, inform other Clients about that
                                    if (ChangedUDDR.RowState == DataRowState.Added)
                                    {
#if DEBUGMODE
                                        if (TSrvSetting.DL >= 8)
                                        {
                                            Console.WriteLine(
                                                "TMaintenanceUserDefaults.SaveUserDefaultsTable: new UserDefault has been INSERTed into the DB Table, inform other Clients about that!");
                                        }
#endif
                                        ASendUpdateInfoToClient = true;
                                        break;
                                    }
                                }

                                if (ASendUpdateInfoToClient)
                                {
                                    // CopyModificationIDsOver(ChangedUserDefaultsDT, AUserDefaultsDataTable);
#if DEBUGMODE
                                    if (TSrvSetting.DL >= 8)
                                    {
                                        Console.WriteLine("TMaintenanceUserDefaults.SaveUserDefaultsTable: informing other Clients!");
                                    }
#endif
                                    UpdateUserDefaultsOnClient(AUserName, ChangedUserDefaults2DT);
                                }
                            }

                            // We have no unsaved changes anymore
                            AUserDefaultsDataTable.AcceptChanges();
                        }

#if DEBUGMODE
                        if (TSrvSetting.DL >= 8)
                        {
                            Console.WriteLine("TMaintenanceUserDefaults.SaveUserDefaultsTable: after AcceptChanges.");
                        }
#endif
                    }
#if DEBUGMODE
                    catch (Exception Exp)
                    {
                        if (TSrvSetting.DL >= 8)
                        {
                            Console.WriteLine("TMaintenanceUserDefaults.SaveUserDefaultsTable: Exception occured: " + Exp.ToString());
                        }

                        throw;
                    }
#endif
                    finally
                    {
                        if (NewTransaction)
                        {
                            if (SubmissionOK)
                            {
                                DBAccess.GDBAccessObj.CommitTransaction();
#if DEBUGMODE
                                if (TSrvSetting.DL >= 8)
                                {
                                    Console.WriteLine("TMaintenanceUserDefaults.SaveUserDefaultsTable: committed own transaction.");
                                }
#endif
                            }
                            else
                            {
                                DBAccess.GDBAccessObj.RollbackTransaction();
#if DEBUGMODE
                                if (TSrvSetting.DL >= 8)
                                {
                                    Console.WriteLine("TMaintenanceUserDefaults.SaveUserDefaultsTable: rolled back own transaction.");
                                }
#endif
                            }
                        }
                    }

#if DEBUGMODE
                    if (TSrvSetting.DL >= 8)
                    {
                        Console.WriteLine("TMaintenanceUserDefaults.SaveUserDefaultsTable: Done!");
                    }
#endif
                    ReturnValue = SubmissionOK;
                }
                else
                {
// nothing to save!
#if DEBUGMODE
                    if (TSrvSetting.DL >= 8)
                    {
                        Console.WriteLine("TMaintenanceUserDefaults.SaveUserDefaultsTable: nothing to save: no changes!");
                    }
#endif
                    ReturnValue = true;
                }
            }
            else
            {
                // nothing to save!
#if DEBUGMODE
                if (TSrvSetting.DL >= 8)
                {
                    Console.WriteLine("TMaintenanceUserDefaults.SaveUserDefaultsTable: nothing to save: no UserDefaults in memory!");
                }
#endif
                ReturnValue = true;
            }

            return ReturnValue;
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="AUserName"></param>
        /// <param name="AUserDefaultsDataTable"></param>
        /// <param name="AWriteTransaction"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        public static Boolean SaveUserDefaultsTable(String AUserName,
            ref SUserDefaultsTable AUserDefaultsDataTable,
            TDBTransaction AWriteTransaction,
            ref TVerificationResultCollection AVerificationResult)
        {
            return SaveUserDefaultsTable(AUserName, ref AUserDefaultsDataTable, AWriteTransaction, ref AVerificationResult, true);
        }

        /// <summary>
        /// Saves changes that were made to the internal User Defaults cache (for the
        /// current user) to the DB.
        /// DB Table: s_user_defaults
        ///
        /// </summary>
        /// <param name="AVerificationResult">Nil if DB calls succeded, otherwise filled with
        /// 1..n TVerificationResult objects (contain DB call exceptions).</param>
        /// <param name="ASendUpdateInfoToClient">If true, the PetraClient is informed about the
        /// change to the UserDefault (by means of sending a ClientTask)</param>
        /// <returns>true if the User Defaults could be saved successfully or if there
        /// was nothing to save. false indicates that a DB call resulted in an error.
        /// Inspect AVerificationResult to retrieve the error.
        /// </returns>
        public static Boolean SaveUserDefaultsFromServerSide(out TVerificationResultCollection AVerificationResult, Boolean ASendUpdateInfoToClient)
        {
            Boolean ReturnValue;
            Boolean SubmissionOK;

            AVerificationResult = null;
#if DEBUGMODE
            if (TSrvSetting.DL >= 7)
            {
                Console.WriteLine("TMaintenanceUserDefaults.SaveUserDefaultsFromServerSide waiting for a ReaderLock...");
            }
#endif

            // Prevent other threads from obtaining a read lock on the cache table while we are reading values from the cache table!
            UReadWriteLock.AcquireReaderLock(SharedConstants.THREADING_WAIT_INFINITE);
#if DEBUGMODE
            if (TSrvSetting.DL >= 7)
            {
                Console.WriteLine("TMaintenanceUserDefaults.SaveUserDefaultsFromServerSide grabbed a ReaderLock.");
            }
#endif
            try
            {
                if (UUserDefaultsDT.Rows.Count > 0)
                {
                    SubmissionOK = TUserDefaults.SaveUserDefaultsTable(UserInfo.GUserInfo.UserID,
                        ref UUserDefaultsDT,
                        null,
                        ref AVerificationResult,
                        ASendUpdateInfoToClient);

                    if (SubmissionOK)
                    {
                        // we don't have any unsaved changes anymore in the cache table.
                        UUserDefaultsDT.AcceptChanges();
                    }

                    ReturnValue = SubmissionOK;
                }
                else
                {
                    // nothing to save!
#if DEBUGMODE
                    if (TSrvSetting.DL >= 8)
                    {
                        Console.WriteLine("TMaintenanceUserDefaults.SaveUserDefaultsFromServerSide: nothing to save.");
                    }
#endif
                    ReturnValue = true;
                }
            }
            finally
            {
                // Other threads are now free to obtain a read lock on the cache table.
                UReadWriteLock.ReleaseReaderLock();
#if DEBUGMODE
                if (TSrvSetting.DL >= 7)
                {
                    Console.WriteLine("TMaintenanceUserDefaults.SaveUserDefaultsFromServerSide released the ReaderLock.");
                }
#endif
            }
#if DEBUGMODE
            if (TSrvSetting.DL >= 8)
            {
                Console.WriteLine("TMaintenanceUserDefaults.SaveUserDefaultsFromServerSide: Done!");
            }
#endif
            return ReturnValue;
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        public static Boolean SaveUserDefaultsFromServerSide(ref TVerificationResultCollection AVerificationResult)
        {
            return SaveUserDefaultsFromServerSide(out AVerificationResult, true);
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
        /// <returns>void</returns>
        public static void SetDefault(String AKey, object AValue, Boolean ASendUpdateInfoToClient)
        {
            TInternal.SetUserDefault(AKey, AValue.ToString(), ASendUpdateInfoToClient);
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="AKey"></param>
        /// <param name="AValue"></param>
        public static void SetDefault(String AKey, object AValue)
        {
            SetDefault(AKey, AValue, true);
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
        public static void UpdateUserDefaultsOnClient(String AUserName,
            String AChangedUserDefaultCode,
            String AChangedUserDefaultValue,
            String AChangedUserDefaultModId,
            Boolean ASingleCode)
        {
            String SingleOrMultipleIndicator;

#if DEBUGMODE
            if (TSrvSetting.DL >= 7)
            {
                Console.WriteLine("TMaintenanceUserDefaults.UpdateUserDefaultsOnClient: calling DomainManager.ClientTaskAdd...");
            }
#endif

            if (AChangedUserDefaultCode == null)
            {
                if (AUserName == UserInfo.GUserInfo.UserID)
                {
                    // Queue a ClientTask to the current User's PetraClient
                    Ict.Petra.Server.App.ClientDomain.DomainManager.ClientTaskAdd(SharedConstants.CLIENTTASKGROUP_USERDEFAULTSREFRESH, "All", 1);
                }
                else
                {
                    // Queue a ClientTask to any but the current User's PetraClient
                    Ict.Petra.Server.App.ClientDomain.DomainManager.ClientTaskAddToOtherClient(AUserName,
                        SharedConstants.CLIENTTASKGROUP_USERDEFAULTSREFRESH,
                        "All",
                        1);
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

                if (AUserName == UserInfo.GUserInfo.UserID)
                {
                    // Queue a ClientTask to the current User's PetraClient
                    Ict.Petra.Server.App.ClientDomain.DomainManager.ClientTaskAdd(SharedConstants.CLIENTTASKGROUP_USERDEFAULTSREFRESH,
                        SingleOrMultipleIndicator,
                        AChangedUserDefaultCode,
                        AChangedUserDefaultValue,
                        AChangedUserDefaultModId,
                        null,
                        1);
                }

                // Send the same ClientTask to all other running PetraClient instances where
                // the same user is logged in!
                Ict.Petra.Server.App.ClientDomain.DomainManager.ClientTaskAddToOtherClient(AUserName,
                    SharedConstants.CLIENTTASKGROUP_USERDEFAULTSREFRESH,
                    SingleOrMultipleIndicator,
                    AChangedUserDefaultCode,
                    AChangedUserDefaultValue,
                    AChangedUserDefaultModId,
                    null,
                    1);
            }
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="AUserName"></param>
        /// <param name="AChangedUserDefaultCode"></param>
        /// <param name="AChangedUserDefaultValue"></param>
        /// <param name="AChangedUserDefaultModId"></param>
        public static void UpdateUserDefaultsOnClient(String AUserName,
            String AChangedUserDefaultCode,
            String AChangedUserDefaultValue,
            String AChangedUserDefaultModId)
        {
            UpdateUserDefaultsOnClient(AUserName, AChangedUserDefaultCode, AChangedUserDefaultValue, AChangedUserDefaultModId, true);
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
                    ChangedUserDefaultCodes = ChangedUserDefaultCodes + TClientTasksManager.GCLIENTTASKPARAMETER_SEPARATOR +
                                              AChangedUserDefaultsDT[Counter].DefaultCode;
                    ChangedUserDefaultValues = ChangedUserDefaultValues + TClientTasksManager.GCLIENTTASKPARAMETER_SEPARATOR +
                                               AChangedUserDefaultsDT[Counter].DefaultValue;
                    ChangedUserDefaultModIds = ChangedUserDefaultModIds + TClientTasksManager.GCLIENTTASKPARAMETER_SEPARATOR +
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

        #region TInternal

        /// <summary>
        /// internal class for user defaults
        /// </summary>
        public class TInternal
        {
            /// <summary>
            /// check if a user default exists with that key name
            /// </summary>
            /// <param name="AKey"></param>
            /// <returns></returns>
            public static bool HasUserDefault(String AKey)
            {
                return GetUserDefault(AKey, SharedConstants.SYSDEFAULT_NOT_FOUND) != SharedConstants.SYSDEFAULT_NOT_FOUND;
            }

            /**
             * Gets the value of a UserDefault.
             *
             * @param AKey The Key of the User Default that should get retrieved
             * @param ADefault The value that should be returned in case the Key is not (yet)
             * in the User Defaults
             * @return Value of the UserDefault, or the value specified in ADefaultValue if
             * the UserDefault doesn't exist yet
             *
             */
            public static String GetUserDefault(String AKey, String ADefaultValue)
            {
                String ReturnValue;
                Int32 FoundInRow;
                SUserDefaultsTable UserDefaultsDataTable = null;

                ReturnValue = "";

                if (!UTableCached)
                {
                    // make sure we have loaded the UserDefaults for the current User...
                    TUserDefaults.GetUserDefaults(UserInfo.GUserInfo.UserID, out UserDefaultsDataTable);
                }

#if DEBUGMODE
                if (TSrvSetting.DL >= 7)
                {
                    Console.WriteLine("TMaintenanceUserDefaults.GetUserDefault waiting for a ReaderLock...");
                }
#endif

                // Prevent other threads from obtaining a read lock on the cache table while we are reading a value from the cache table!
                UReadWriteLock.AcquireReaderLock(SharedConstants.THREADING_WAIT_INFINITE);
#if DEBUGMODE
                if (TSrvSetting.DL >= 7)
                {
                    Console.WriteLine("TMaintenanceUserDefaults.GetUserDefault grabbed a ReaderLock.");
                }
#endif
                try
                {
                    FoundInRow = UUserDefaultsDV.Find(AKey);

                    if (FoundInRow != -1)
                    {
                        // User default found > read it
                        ReturnValue = (UUserDefaultsDV[FoundInRow][SUserDefaultsTable.GetDefaultValueDBName()]).ToString();
                    }
                    else
                    {
                        // User default not found, return default value
                        ReturnValue = ADefaultValue;
                    }
                }
                finally
                {
                    // Other threads are now free to obtain a read lock on the cache table.
                    UReadWriteLock.ReleaseReaderLock();
#if DEBUGMODE
                    if (TSrvSetting.DL >= 7)
                    {
                        Console.WriteLine("TMaintenanceUserDefaults.GetUserDefault released the ReaderLock.");
                    }
#endif
                }
                return ReturnValue;
            }

            /// <summary>
            /// overload
            /// </summary>
            /// <param name="AKey"></param>
            /// <returns></returns>
            public static String GetUserDefault(String AKey)
            {
                return GetUserDefault(AKey, "");
            }

            /**
             * Sets a UserDefault to a certain value.
             *
             * @param AKey The Key of the UserDefault that should get changed
             * @param AValue Value that the UserDefault should be changed to
             * @param ASendUpdateInfoToClient If true, the PetraClient is informed about
             * the change to the UserDefault (by means of sending a ClientTask)
             *
             */
            public static void SetUserDefault(String AKey, String AValue, Boolean ASendUpdateInfoToClient)
            {
                Int32 FoundInRow;
                DataRowView Tmp;
                SUserDefaultsTable UserDefaultsDataTable;

                if (!UTableCached)
                {
                    // make sure we have loaded the UserDefaults for the current User...
                    TUserDefaults.GetUserDefaults(UserInfo.GUserInfo.UserID, out UserDefaultsDataTable);
                }

#if DEBUGMODE
                if (TSrvSetting.DL >= 7)
                {
                    Console.WriteLine("TMaintenanceUserDefaults.SetUserDefault waiting for a WriterLock...");
                }
#endif

                // Prevent other threads from obtaining a read lock on the cache table while we are changing a value in the cache table!
                UReadWriteLock.AcquireWriterLock(SharedConstants.THREADING_WAIT_INFINITE);
#if DEBUGMODE
                if (TSrvSetting.DL >= 7)
                {
                    Console.WriteLine("TMaintenanceUserDefaults.SetUserDefault grabbed a WriterLock.");
                }
#endif
                FoundInRow = UUserDefaultsDV.Find(AKey);
                try
                {
                    if (FoundInRow != -1)
                    {
                        // User default found
                        if (AValue != UUserDefaultsDV[FoundInRow][SUserDefaultsTable.GetDefaultValueDBName()].ToString())
                        {
                            // Update only if the value is actually different
                            UUserDefaultsDV[FoundInRow][SUserDefaultsTable.GetDefaultValueDBName()] = AValue;
#if DEBUGMODE
                            if (TSrvSetting.DL >= 7)
                            {
                                Console.WriteLine(
                                    "TMaintenanceUserDefaults.SetUserDefault: updated UserDefault '" + AKey + "' with value '" + AValue + "'.");
                            }
#endif
                        }
                    }
                    else
                    {
                        // User default not found, add it to the user defaults table
                        Tmp = UUserDefaultsDV.AddNew();
                        Tmp[SUserDefaultsTable.GetUserIdDBName()] = UserInfo.GUserInfo.UserID;
                        Tmp[SUserDefaultsTable.GetDefaultCodeDBName()] = AKey;
                        Tmp[SUserDefaultsTable.GetDefaultValueDBName()] = AValue;
                        Tmp.EndEdit();
#if DEBUGMODE
                        if (TSrvSetting.DL >= 7)
                        {
                            Console.WriteLine(
                                "TMaintenanceUserDefaults.SetUserDefault: added UserDefault '" + AKey + "' with value '" + AValue + "'.");
                        }
#endif
                        FoundInRow = UUserDefaultsDV.Find(AKey);
                    }

                    if (ASendUpdateInfoToClient)
                    {
                        TUserDefaults.UpdateUserDefaultsOnClient(UserInfo.GUserInfo.UserID, AKey, AValue,
                            UUserDefaultsDV[FoundInRow][SUserDefaultsTable.GetModificationIdDBName()].ToString());
                    }
                }
                finally
                {
                    // Other threads are now free to obtain a read lock on the cache table.
                    UReadWriteLock.ReleaseWriterLock();
#if DEBUGMODE
                    if (TSrvSetting.DL >= 7)
                    {
                        Console.WriteLine("TMaintenanceUserDefaults.SetUserDefault released the WriterLock.");
                    }
#endif
                }
            }

            /// <summary>
            /// overload
            /// </summary>
            /// <param name="AKey"></param>
            /// <param name="AValue"></param>
            public static void SetUserDefault(String AKey, String AValue)
            {
                SetUserDefault(AKey, AValue, true);
            }
        }
        #endregion
    }
}