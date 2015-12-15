//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Data.Odbc;
using System.Threading;

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MSysMan.Data.Access;

namespace Ict.Petra.Server.App.Core
{
    /// <summary>
    /// Cache Manager for the System Defaults of the Petra Site.
    /// It is instantiated only once (in ServerManager) and then
    /// accessed from each ClientDomain.
    ///
    /// The SystemDefaults are read from the DB when they are first requested,
    /// all subsequent requests just use the cached System Defaults. This is fully
    /// transparent to the caller. A reload of the cached System Defaults table can
    /// be requested. Read access to the cache table is denied while the cache table
    /// is (re)loaded to allow safe multi-threading operation.
    ///
    /// @Comment The System Defaults are retrieved from the s_system_defaults table
    ///   and are put into a Typed DataTable that has the structure of this table.
    /// </summary>
    public class TSystemDefaultsCache : ISystemDefaultsCache
    {
        /// a static variable for global access to the system defaults
        public static TSystemDefaultsCache GSystemDefaultsCache;

        /*------------------------------------------------------------------------------
         * Partner System Default Constants
         * -------------------------------------------------------------------------------*/

        /// <summary>Find Screen Options</summary>
        public const String PARTNER_GIFTRECEIPTINGDEFAULTS = "GiftReceiptingDefaults";

        /*------------------------------------------------------------------------------
         * Put other User Default Constants here as well.
         * -------------------------------------------------------------------------------*/

        /// <summary>holds a state that tells whether the Typed DataTable is cached or not</summary>
        private Boolean FTableCached;

        /// <summary>this Typed DataTable holds the cached System Defaults</summary>
        private SSystemDefaultsTable FSystemDefaultsDT;

        /// <summary>used to control read and write access to the cache</summary>
        private System.Threading.ReaderWriterLock FReadWriteLock;

        #region TSystemDefaultsCache

        /// <summary>
        /// constructor
        /// </summary>
        public TSystemDefaultsCache() : base()
        {
            FReadWriteLock = new System.Threading.ReaderWriterLock();
        }

        /// <summary>
        /// Returns the System Defaults as a Typed DataTable.
        ///
        /// The caller doesn't need to know whether the Cache is already populated - if
        /// this should be necessary, this function will make a request to populate the
        /// cache.
        ///
        /// </summary>
        /// <returns>System Defaults as a Typed DataTable.
        /// </returns>
        public SSystemDefaultsTable GetSystemDefaultsTable()
        {
            SSystemDefaultsTable ReturnValue;

            // FIXME Inquiring FTableCached (and potentially updating) isn't done in a thread-safe manner - it needs to be 
            // made thread-safe by using a lock!
            if (!FTableCached)
            {
                LoadSystemDefaultsTable();
                
                FTableCached = true;
            }

            try
            {
                /*
                 * Try to get a read lock on the cache table [We don't specify a timeout because
                 *   (1) reading an emptied cache would lead to problems (it is emptied before the DB queries are issued),
                 *   (2) reading the DB tables into the cached table should be fairly quick]
                 */
                FReadWriteLock.AcquireReaderLock(SharedConstants.THREADING_WAIT_INFINITE);
                ReturnValue = FSystemDefaultsDT;
            }
            finally
            {
                // Release read lock on the cache table
                FReadWriteLock.ReleaseReaderLock();
            }
            return ReturnValue;
        }

        /// <summary>
        /// Returns the value of the specified System Default.
        ///
        /// The caller doesn't need to know whether the Cache is already populated - if
        /// this should be necessary, this function will make a request to populate the
        /// cache.
        ///
        /// </summary>
        /// <param name="ASystemDefaultName">The System Default for which the value should be
        /// returned</param>
        /// <returns>The value of the System Default, or SYSDEFAULT_NOT_FOUND if the
        /// specified System Default doesn't exist
        /// </returns>
        private String GetSystemDefault(String ASystemDefaultName)
        {
            String ReturnValue;
            SSystemDefaultsRow FoundSystemDefaultsRow;

            // FIXME Inquiring FTableCached (and potentially updating) isn't done in a thread-safe manner - it needs to be 
            // made thread-safe by using a lock!
            if (!FTableCached)
            {
                LoadSystemDefaultsTable();
                
                FTableCached = true;
            }

            try
            {
                /*
                 * Try to get a read lock on the cache table [We don't specify a timeout because
                 *   (1) reading an emptied cache would lead to problems (it is emptied before the DB queries are issued),
                 *   (2) reading the DB tables into the cached table should be fairly quick]
                 */
                FReadWriteLock.AcquireReaderLock(SharedConstants.THREADING_WAIT_INFINITE);

                FoundSystemDefaultsRow = (SSystemDefaultsRow)FSystemDefaultsDT.Rows.Find(ASystemDefaultName);

                if (FoundSystemDefaultsRow != null)
                {
                    ReturnValue = FoundSystemDefaultsRow.DefaultValue;
                }
                else
                {
                    ReturnValue = SharedConstants.SYSDEFAULT_NOT_FOUND;
                }
            }
            finally
            {
                // Release read lock on the cache table
                FReadWriteLock.ReleaseReaderLock();
            }
            return ReturnValue;
        }

        /// <summary>
        /// Returns the value of the specified System Default.
        ///
        /// The caller doesn't need to know whether the Cache is already populated - if
        /// this should be necessary, this function will make a request to populate the
        /// cache.
        ///
        /// </summary>
        /// <param name="ASystemDefaultName">The System Default for which the value should be
        /// returned</param>
        /// <param name="ADefault">The value that should be returned if the System Default was
        /// not found</param>
        /// <returns>The value of the System Default, or the value of the ADefault
        /// parameter if the specified System Default was not found
        /// </returns>
        private String GetSystemDefault(String ASystemDefaultName, String ADefault)
        {
            String ReturnValue;
            String Tmp;

            Tmp = GetSystemDefault(ASystemDefaultName);

            if (Tmp != SharedConstants.SYSDEFAULT_NOT_FOUND)
            {
                ReturnValue = Tmp;
            }
            else
            {
                ReturnValue = ADefault;
            }

            return ReturnValue;
        }

        /// <summary>
        /// get boolean default value
        /// </summary>
        /// <param name="AKey"></param>
        /// <param name="ADefault"></param>
        /// <returns></returns>
        public bool GetBooleanDefault(String AKey, bool ADefault)
        {
            return Convert.ToBoolean(GetSystemDefault(AKey, ADefault.ToString()));
        }

        /// <summary>
        /// get boolean default value
        /// </summary>
        /// <param name="AKey"></param>
        /// <returns>true if the key does not exist, otherwise value of key</returns>
        public bool GetBooleanDefault(String AKey)
        {
            return GetBooleanDefault(AKey, true);
        }

        /// <summary>
        /// get char default
        /// </summary>
        /// <param name="AKey"></param>
        /// <param name="ADefault"></param>
        /// <returns></returns>
        public System.Char GetCharDefault(String AKey, System.Char ADefault)
        {
            return Convert.ToChar(GetSystemDefault(AKey, ADefault.ToString()));
        }

        /// <summary>
        /// get char default
        /// </summary>
        /// <param name="AKey"></param>
        /// <returns>space if key does not exist</returns>
        public System.Char GetCharDefault(String AKey)
        {
            return GetCharDefault(AKey, ' ');
        }

        /// <summary>
        /// get double default
        /// </summary>
        /// <param name="AKey"></param>
        /// <param name="ADefault"></param>
        /// <returns></returns>
        public double GetDoubleDefault(String AKey, double ADefault)
        {
            return Convert.ToDouble(GetSystemDefault(AKey, ADefault.ToString()));
        }

        /// <summary>
        /// get double default
        /// </summary>
        /// <param name="AKey"></param>
        /// <returns>0.0 if key does not exist</returns>
        public double GetDoubleDefault(String AKey)
        {
            return GetDoubleDefault(AKey, 0.0);
        }

        /// <summary>
        /// Put other User Default Constants here as well.
        /// -------------------------------------------------------------------------------}// ...{
        /// The following set of functions serve as shortcuts to get User Defaults of a
        /// specific type.
        ///
        /// </summary>
        /// <param name="AKey">The Key of the User Default that should get retrieved.</param>
        /// <param name="ADefault">The value that should be returned in case the Key is not (yet)
        /// in the User Defaults.
        /// </param>
        /// <returns>void</returns>
        public System.Int16 GetInt16Default(String AKey, System.Int16 ADefault)
        {
            return Convert.ToInt16(GetSystemDefault(AKey, ADefault.ToString()));
        }

        /// <summary>
        /// get int default
        /// </summary>
        /// <param name="AKey"></param>
        /// <returns>0 if key does not exist</returns>
        public System.Int16 GetInt16Default(String AKey)
        {
            return GetInt16Default(AKey, 0);
        }

        /// <summary>
        /// get int default
        /// </summary>
        /// <param name="AKey"></param>
        /// <param name="ADefault"></param>
        /// <returns></returns>
        public System.Int32 GetInt32Default(String AKey, System.Int32 ADefault)
        {
            return Convert.ToInt32(GetSystemDefault(AKey, ADefault.ToString()));
        }

        /// <summary>
        /// get int default
        /// </summary>
        /// <param name="AKey"></param>
        /// <returns>0 if key does not exist</returns>
        public System.Int32 GetInt32Default(String AKey)
        {
            return GetInt32Default(AKey, 0);
        }

        /// <summary>
        /// get int default
        /// </summary>
        /// <param name="AKey"></param>
        /// <param name="ADefault"></param>
        /// <returns></returns>
        public System.Int64 GetInt64Default(String AKey, System.Int64 ADefault)
        {
            return Convert.ToInt64(GetSystemDefault(AKey, ADefault.ToString()));
        }

        /// <summary>
        /// get int default
        /// </summary>
        /// <param name="AKey"></param>
        /// <returns>0 if key does not exist</returns>
        public System.Int64 GetInt64Default(String AKey)
        {
            return GetInt64Default(AKey, 0);
        }

        /// <summary>
        /// get string default
        /// </summary>
        /// <param name="AKey"></param>
        /// <param name="ADefault"></param>
        /// <returns></returns>
        public String GetStringDefault(String AKey, String ADefault)
        {
            return GetSystemDefault(AKey, ADefault);
        }

        /// <summary>
        /// get string default
        /// </summary>
        /// <param name="AKey"></param>
        /// <returns>empty string if key does not exist</returns>
        public String GetStringDefault(String AKey)
        {
            return GetStringDefault(AKey, "");
        }

        /// <summary>
        /// Loads the System Defaults into the cached Typed DataTable.
        ///
        /// The System Defaults are retrieved from the s_system_defaults table and are
        /// put into a Typed DataTable that has the structure of this table.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void LoadSystemDefaultsTable()
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction = false;

            // Prevent other threads from obtaining a read lock on the cache table while we are (re)loading the cache table!
            FReadWriteLock.AcquireWriterLock(SharedConstants.THREADING_WAIT_INFINITE);

            try
            {
                if (FSystemDefaultsDT != null)
                {
                    FSystemDefaultsDT.Clear();
                }

                try
                {
                    ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.RepeatableRead,
                        TEnforceIsolationLevel.eilMinimum,
                        out NewTransaction);
                    FSystemDefaultsDT = SSystemDefaultsAccess.LoadAll(ReadTransaction);
                }
                finally
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
                        TLogging.LogAtLevel(7, "TSystemDefaultsCache.LoadSystemDefaultsTable: commited own transaction.");
                    }
                }

                // Thread.Sleep(5000);     uncomment this for debugging. This allows checking whether read access to FSystemDefaultsDT actually waits until we release the WriterLock in the finally block.
            }
            finally
            {
                // Other threads are now free to obtain a read lock on the cache table.
                FReadWriteLock.ReleaseWriterLock();
            }
        }

        /// <summary>
        /// Reloads the cached TypedDataTable that holds the System Defaults immediately.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void ReloadSystemDefaultsTable()
        {
            LoadSystemDefaultsTable();
        }

        /// <summary>
        /// Saves changes to the submitted Typed SystemDefaults DataTable to the DB.
        ///
        /// @comment Currently always returns false because the function needs to be
        /// rewritten!
        ///
        /// @todo Rewrite this function so that it saves entries that originally came
        /// from the s_system_parameter table in the DB to this table instead of
        /// writing them to the s_system_defaults table! Also use the DataStore to
        /// save the data instead of using SQL queries!!!
        ///
        /// </summary>
        /// <param name="ASystemDefaultsDataTable">Typed SystemDefaults DataTable</param>
        /// <returns>true if the System Defaults could be saved successfully
        /// </returns>
        public Boolean SaveSystemDefaults(SSystemDefaultsTable ASystemDefaultsDataTable)
        {
            // var
            // Transaction: OdbcTransaction;
            // ParametersArray: array of OdbcParameter;
            // Counter,
            // DefaultInDataBaseCount: Int16;
            // AllSubmissionsOK: Boolean;

            /* TODO 2 oChristanK cDB : Rewrite this function so that it saves entries that originally came from the s_system_parameter table
             * in the DB to this table instead of writing them to the s_system_defaults table! Also use the DataStore to
             * save the data instead of using SQL queries!!!
             */

            // Currently always returns false because the function needs to be rewritten!
            return false;

            // AllSubmissionsOK := false;
            // DefaultInDataBaseCount := 0;
            //
            // if (ASystemDefaultsDataTable <> nil) and (ASystemDefaultsDataTable.Rows.Count > 0) then
            // begin
            // $IFDEF DEBUGMODE if TLogging.DL >= 8 then Console.WriteLine('Saving ' + (ASystemDefaultsDataTable.Rows.Count).ToString + ' System Defaults...'); $ENDIF
            //
            // Transaction := DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);
            //
            // Loop over all changed/added System Defaults
            // for Counter := 0 to ASystemDefaultsDataTable.Rows.Count  1 do
            // begin
            // Look whether the System Default already exists in the DB
            // SetLength(ParametersArray, 1);
            // ParametersArray[0] := new OdbcParameter('', OdbcType.VarChar, 32);
            // ParametersArray[0].Value := ASystemDefaultsDataTable.Rows[Counter].Item['s_default_code_c'].ToString;
            //
            // try
            // DefaultInDataBaseCount := Convert.ToInt16( DBAccess.GDBAccessObj.ExecuteScalar(
            // 'SELECT COUNT) ' +
            // 'FROM PUB_s_system_defaults ' +
            // 'WHERE s_default_code_c = ?', Transaction, false, ParametersArray) );
            // except
            // on exp: Exception do
            // begin
            // Result := false;
            // $IFDEF DEBUGMODE if TLogging.DL >= 9 then Console.WriteLine(this.GetType.FullName + '.SaveSystemDefaults: Error running count query!!! ' +
            // 'Possible cause: ' + exp.ToString); $ENDIF
            // Exit;
            // end;
            // end;
            //
            // if DefaultInDataBaseCount = 0 then
            // begin
            // System Default doesn't exist yet > create it

            /* $IFDEF DEBUGMODE if TLogging.DL >= 8 then Console.WriteLine('Inserting SystemDefault ' + ASystemDefaultsDataTable.Rows[Counter].Item['s_default_code_c'].ToString + '; Value: ' +
             *ASystemDefaultsDataTable.Rows[Counter].Item['s_default_value_c'].ToString);$ENDIF */

            // SetLength(ParametersArray, 3);
            // ParametersArray[0] := new OdbcParameter('', OdbcType.VarChar, 32);
            // ParametersArray[0].Value := ASystemDefaultsDataTable.Rows[Counter].Item['s_default_code_c'].ToString;
            // ParametersArray[0] := new OdbcParameter('', OdbcType.VarChar, 48);
            // ParametersArray[0].Value := ASystemDefaultsDataTable.Rows[Counter].Item['s_default_description_c'].ToString;
            // ParametersArray[2] := new OdbcParameter('', OdbcType.VarChar, 96);
            // ParametersArray[2].Value := ASystemDefaultsDataTable.Rows[Counter].Item['s_default_value_c'].ToString;
            //
            // try
            // DBAccess.GDBAccessObj.ExecuteNonQuery(
            // 'INSERT INTO PUB_s_system_defaults ' +
            // '(s_default_code_c, s_default_description_c, s_default_value_c) ' +
            // 'VALUES (?, ?, ?)', Transaction, false, ParametersArray);
            // except
            // on exp: Exception do
            // begin
            // Result := false;
            // $IFDEF DEBUGMODE if TLogging.DL >= 9 then Console.WriteLine(this.GetType.FullName + '.SaveSystemDefaults: Error running insert query!!! ' +
            // 'Possible cause: ' + exp.ToString); $ENDIF
            // Exit;
            // end;
            // end;
            // end
            // else
            // begin
            // System Default exists > update it

            /* $IFDEF DEBUGMODE if TLogging.DL >= 8 then Console.WriteLine('Updating SystemDefault ' + ASystemDefaultsDataTable.Rows[Counter].Item['s_default_code_c'].ToString + '; Value: ' +
             *ASystemDefaultsDataTable.Rows[Counter].Item['s_default_value_c'].ToString);$ENDIF */

            // SetLength(ParametersArray, 3);
            // ParametersArray[0] := new OdbcParameter('', OdbcType.VarChar, 48);
            // ParametersArray[0].Value := ASystemDefaultsDataTable.Rows[Counter].Item['s_default_description_c'];
            // ParametersArray[1] := new OdbcParameter('', OdbcType.VarChar, 96);
            // ParametersArray[1].Value := ASystemDefaultsDataTable.Rows[Counter].Item['s_default_value_c'];
            // ParametersArray[2] := new OdbcParameter('', OdbcType.VarChar, 32);
            // ParametersArray[2].Value := ASystemDefaultsDataTable.Rows[Counter].Item['s_default_code_c'];
            //
            // try
            // DBAccess.GDBAccessObj.ExecuteNonQuery(
            // 'UPDATE PUB_s_system_defaults ' +
            // 'SET s_default_description_c = ?, s_default_value_c = ? ' +
            // 'WHERE s_default_code_c = ?', Transaction, false, ParametersArray);
            // except
            // on exp: Exception do
            // begin
            // Result := false;
            // $IFDEF DEBUGMODE if TLogging.DL >= 9 then Console.WriteLine(this.GetType.FullName + '.SaveSystemDefaults: Error runing insert query!!! ' +
            // 'Possible cause: ' + exp.ToString); $ENDIF
            // Exit;
            // end;
            // end;
            // end;
            // end;
            //
            // DBAccess.GDBAccessObj.CommitTransaction;
            // Result := AllSubmissionsOK;
            // end
            // else
            // begin
            // nothing to save!
            // Result := false;
            // end;
        }

        #endregion
    }
}