//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2016 by OM International
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

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MSysMan.Data.Access;
using Ict.Common.Remoting.Server;

namespace Ict.Petra.Server.App.Core.Security
{
    /// <summary>
    /// Saves entries in the Login Log table.
    /// </summary>
    public class TLoginLog : ILoginLog
    {
        /// <summary>User logout.</summary>
        public const string LOGIN_STATUS_TYPE_LOGOUT = "LOGOUT";
        // See also constants LOGIN_STATUS_TYPE_* in Ict.Petra.Server.MSysMan.Security.TLoginLog!

        /// <summary>
        /// Adds a record to the s_login DB Table. That DB Table contains a log of all the log-ins/log-in attempts to
        /// the system, and of log-outs from the system.
        /// </summary>
        /// <param name="AUserID">UserID of the User for which a record should be written.</param>
        /// <param name="ALoginType">Type of the login/logout record. This is a hard-coded constant value
        /// (there's no 'lookup table' for it); for available values and their meaning please check program code
        /// (Ict.Petra.Server.MSysMan.Security.TLoginLog Class).</param>
        /// <param name="ALoginDetails">Details/description of the login/login attempt/logout </param>
        /// <param name="AProcessID">'Process ID'; this is a unique key and comes from a sequence (seq_login_process_id).</param>
        /// <param name="ATransaction">Instantiated DB Transaction.</param>
        public static void AddLoginLogEntry(String AUserID, String ALoginType, String ALoginDetails, out Int32 AProcessID,
            TDBTransaction ATransaction)
        {
            SLoginTable LoginTable = new SLoginTable();
            SLoginRow NewLoginRow = LoginTable.NewRowTyped(false);

            OdbcParameter[] ParametersArray;
            DateTime LoginDateTime = DateTime.Now;

            // Set DataRow values
            NewLoginRow.LoginProcessId = -1;
            NewLoginRow.UserId = AUserID.ToUpper();
            NewLoginRow.LoginType = ALoginType;
            NewLoginRow.LoginDetails = ALoginDetails;
            NewLoginRow.Date = LoginDateTime.Date;
            NewLoginRow.Time = Conversions.DateTimeToInt32Time(LoginDateTime);

            //TLogging.Log(String.Format("AddLoginLogEntry: NewLoginRow.Date: {0}; NewLoginRow.Time: {1}",
            //    NewLoginRow.Date, NewLoginRow.Time));

            LoginTable.Rows.Add(NewLoginRow);

            try
            {
                SLoginAccess.SubmitChanges(LoginTable, ATransaction);
            }
            catch (Exception Exc)
            {
                TLogging.Log("AddLoginLogEntry: An Exception occured during the saving of a Login Log entry (Situation #1):" +
                    Environment.NewLine + Exc.ToString());

                throw;
            }

            // Retrieve ROWID of the SLogin record
            ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 20);
            ParametersArray[0].Value = (System.Object)(AUserID);
            ParametersArray[1] = new OdbcParameter("", OdbcType.Date);
            ParametersArray[1].Value = (System.Object)(LoginDateTime.Date);
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = (System.Object)(NewLoginRow.Time);
            ParametersArray[3] = new OdbcParameter("", OdbcType.VarChar, 500);
            ParametersArray[3].Value = (System.Object)(ALoginDetails);

            try
            {
                // ROWID for postgresql: see http://archives.postgresql.org/sydpug/2005-05/msg00002.php
                AProcessID =
                    Convert.ToInt32(ATransaction.DataBaseObj.ExecuteScalar("SELECT " + SLoginTable.GetLoginProcessIdDBName() + " FROM PUB_" +
                            TTypedDataTable.GetTableNameSQL(SLoginTable.TableId) +
                            ' ' +
                            "WHERE " + SLoginTable.GetUserIdDBName() + " = ? AND " + SLoginTable.GetDateDBName() + " = ? AND " +
                            SLoginTable.GetTimeDBName() + " = ? AND " + SLoginTable.GetLoginDetailsDBName() + " = ?", ATransaction,
                            ParametersArray));
            }
            catch (Exception Exc)
            {
                TLogging.Log("An Exception occured during the saving of the a Login Log entry (Situation #2):" +
                    Environment.NewLine + Exc.ToString());

                throw;
            }
        }

        /// <summary>
        /// Records the logging-out (=disconnection) of a Client to the s_login DB Table. That DB Table contains a log
        /// of all the log-ins/log-in attempts to the system, and of log-outs from the system.
        /// </summary>
        /// <param name="AUserID">UserID of the User for which a logout should be recorded.</param>
        /// <param name="AProcessID">ProcessID of the User for which a logout should be recorded.
        /// This will need to be the number that got returned from an earlier call to
        /// <see cref="AddLoginLogEntry(string, string, string, out int, TDBTransaction)"/>!</param>
        /// <param name="ATransaction">Either an instantiated DB Transaction, or null. In the latter case
        /// a separate DB Connection gets opened, a DB Transaction on that separate DB Connection gets started,
        /// then committed/rolled back and the separate DB Connection gets closed. This is needed when this Method
        /// gets called from Method 'Ict.Common.Remoting.Server.TDisconnectClientThread.StartClientDisconnection()'!</param>
        public void RecordUserLogout(String AUserID, int AProcessID, TDBTransaction ATransaction)
        {
            TDataBase DBConnectionObj = null;
            TDBTransaction WriteTransaction = null;
            SLoginTable LoginTable = new SLoginTable();
            SLoginRow NewLoginRow = LoginTable.NewRowTyped(false);
            bool SubmissionOK = false;
            DateTime LogoutDateTime = DateTime.Now;

            // Set DataRow values
            NewLoginRow.LoginProcessId = -1;
            NewLoginRow.UserId = AUserID.ToUpper();
            NewLoginRow.LoginType = LOGIN_STATUS_TYPE_LOGOUT;
            NewLoginRow.LoginDetails = Catalog.GetString("User logout.");
            NewLoginRow.LoginProcessIdRef = AProcessID;
            NewLoginRow.Date = LogoutDateTime.Date;
            NewLoginRow.Time = Conversions.DateTimeToInt32Time(LogoutDateTime);

            //TLogging.Log(String.Format("RecordUserLogout: NewLoginRow.Date: {0}; NewLoginRow.Time: {1}",
            //    NewLoginRow.Date, NewLoginRow.Time));

            LoginTable.Rows.Add(NewLoginRow);

            if (ATransaction == null)
            {
                // Open a separate DB Connection (necessary because this Method gets executed in the Server's (Main) AppDomain
                // which hasn't got an instance of DBAccess.GDBAccess!) ...
                DBConnectionObj = DBAccess.SimpleEstablishDBConnection("RecordUserLogout");

                // ...and start a DB Transaction on that separate DB Connection
                WriteTransaction = DBConnectionObj.BeginTransaction(IsolationLevel.RepeatableRead, 0, "RecordUserLogout");
            }
            else
            {
                DBConnectionObj = ATransaction.DataBaseObj;
                WriteTransaction = ATransaction;
            }

            try
            {
                try
                {
                    SLoginAccess.SubmitChanges(LoginTable, WriteTransaction);

                    SubmissionOK = true;
                }
                catch (Exception Exc)
                {
                    TLogging.Log("RecordUserLogout: An Exception occured during the saving of a Login Log entry:" +
                        Environment.NewLine + Exc.ToString());

                    throw;
                }
            }
            finally
            {
                if (SubmissionOK)
                {
                    DBConnectionObj.CommitTransaction();
                }
                else
                {
                    DBConnectionObj.RollbackTransaction();
                }

                if (DBConnectionObj != null)
                {
                    DBConnectionObj.CloseDBConnection();
                }
            }
        }
    }
}