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
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MSysMan.Data.Access;
using Ict.Common.Exceptions;
using Ict.Common.Remoting.Server;

namespace Ict.Petra.Server.App.Core.Security
{
    /// <summary>
    /// Saves entries in the Login Log table.
    /// </summary>
    public class TLoginLog : ILoginLog
    {
        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AUserID"></param>
        /// <param name="ALoginSuccesful"></param>
        /// <param name="ALoginStatusType"></param>
        /// <param name="ALoginStatus"></param>
        /// <param name="AProcessID"></param>
        /// <param name="ATransaction">Instantiated DB Transaction.</param>
        public static void AddLoginLogEntry(String AUserID,
            Boolean ALoginSuccesful,
            String ALoginStatusType,
            String ALoginStatus,
            out Int32 AProcessID,
            TDBTransaction ATransaction)
        {
            AddLoginLogEntry(AUserID, ALoginSuccesful, ALoginStatusType, ALoginStatus, false, out AProcessID, ATransaction);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AUserID"></param>
        /// <param name="ALoginSuccesful"></param>
        /// <param name="ALoginStatusType"></param>
        /// <param name="ALoginStatus"></param>
        /// <param name="AImmediateLogout"></param>
        /// <param name="AProcessID"></param>
        /// <param name="ATransaction">Instantiated DB Transaction.</param>
        public static void AddLoginLogEntry(String AUserID,
            Boolean ALoginSuccesful,
            String ALoginStatusType,
            String ALoginStatus,
            Boolean AImmediateLogout,
            out Int32 AProcessID,
            TDBTransaction ATransaction)
        {
            SLoginTable LoginTable = new SLoginTable();
            SLoginRow NewLoginRow = LoginTable.NewRowTyped(false);

            OdbcParameter[] ParametersArray;
            DateTime LoginDateTime = DateTime.Now;

            // Set DataRow values
            NewLoginRow.LoginProcessId = -1;
            NewLoginRow.UserId = AUserID.ToUpper();
            NewLoginRow.LoginSuccessful = ALoginSuccesful;
            NewLoginRow.LoginStatusType = ALoginStatusType;
            NewLoginRow.LoginStatus = ALoginStatus;
            NewLoginRow.LoginDate = LoginDateTime.Date;
            NewLoginRow.LoginTime = Conversions.DateTimeToInt32Time(LoginDateTime);

            if (AImmediateLogout)
            {
                NewLoginRow.LogoutDate = LoginDateTime;
                NewLoginRow.LogoutTime = Conversions.DateTimeToInt32Time(LoginDateTime);
            }

            LoginTable.Rows.Add(NewLoginRow);

            try
            {
                // especially in the unit tests, we need to allow several logins per minute, without unique key violation
                while (SLoginAccess.Exists(NewLoginRow.UserId, NewLoginRow.LoginDate, NewLoginRow.LoginTime, ATransaction))
                {
                    NewLoginRow.LoginTime++;
                }

                SLoginAccess.SubmitChanges(LoginTable, ATransaction);
            }
            catch (Exception Exc)
            {
                TLogging.Log("An Exception occured during the saving of a Login Log entry (Situation #1):" +
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
            ParametersArray[2].Value = (System.Object)(NewLoginRow.LoginTime);
            ParametersArray[3] = new OdbcParameter("", OdbcType.VarChar, 500);
            ParametersArray[3].Value = (System.Object)(ALoginStatus);

            try
            {
                // ROWID for postgresql: see http://archives.postgresql.org/sydpug/2005-05/msg00002.php
                AProcessID =
                    Convert.ToInt32(ATransaction.DataBaseObj.ExecuteScalar("SELECT " + SLoginTable.GetLoginProcessIdDBName() + " FROM PUB_" +
                            TTypedDataTable.GetTableNameSQL(SLoginTable.TableId) +
                            ' ' +
                            "WHERE " + SLoginTable.GetUserIdDBName() + " = ? AND " + SLoginTable.GetLoginDateDBName() + " = ? AND " +
                            SLoginTable.GetLoginTimeDBName() + " = ? AND " + SLoginTable.GetLoginStatusDBName() + " = ?", ATransaction,
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
        /// Records the logging-out (=disconnection) of a Client.
        /// </summary>
        /// <param name="AUserID">UserID of the User for which a logout should be recorded.</param>
        /// <param name="AProcessID">ProcessID of the User for which a logout should be recorded.
        /// This will need to be the number that got returned from an earlier call to
        /// <see cref="AddLoginLogEntry(string, bool, string, string, bool, out int, TDBTransaction)"/>!</param>
        public void RecordUserLogout(String AUserID, int AProcessID)
        {
            TDataBase DBConnectionObj;
            TDBTransaction ReadWriteTransaction = null;
            SLoginTable LoginTable;
            SLoginRow TemplateRow;
            SLoginRow LoginRowForUser;
            bool SubmissionOK = false;
            DateTime LogoutDateTime = DateTime.Now;

            TemplateRow = new SLoginTable().NewRowTyped(false);
            TemplateRow.LoginProcessId = AProcessID;
            TemplateRow.UserId = AUserID;

            // Open a separate DB Connection (necessary because this Method gets executed in the Server's (Main) AppDomain
            // which hasn't got an instance of DBAccess.GDBAccess!) ...
            DBConnectionObj = DBAccess.SimpleEstablishDBConnection("RecordUserLogout");

            // ...and start a DB Transaction on that separate DB Connection
            ReadWriteTransaction = DBConnectionObj.BeginTransaction(IsolationLevel.Serializable, 0, "RecordUserLogout");

            try
            {
                LoginTable = SLoginAccess.LoadUsingTemplate(TemplateRow, ReadWriteTransaction);
                LoginTable.AcceptChanges();

                if (LoginTable.Rows.Count != 1)
                {
                    throw new EOPAppException(String.Format(
                            "An attempt was made to record the logout of user {0} but no login record was found with ProcessID {1}",
                            AUserID, AProcessID));
                }
                else
                {
                    LoginRowForUser = LoginTable[0];

                    LoginRowForUser.LogoutDate = LogoutDateTime;
                    LoginRowForUser.LogoutTime = Conversions.DateTimeToInt32Time(LogoutDateTime);
                }

                SLoginAccess.SubmitChanges(LoginTable, ReadWriteTransaction);

                SubmissionOK = true;
            }
            finally
            {
                if (DBConnectionObj != null)
                {
                    if (SubmissionOK)
                    {
                        DBConnectionObj.CommitTransaction();
                    }
                    else
                    {
                        DBConnectionObj.RollbackTransaction();
                    }

                    DBConnectionObj.CloseDBConnection();
                }
            }
        }
    }
}