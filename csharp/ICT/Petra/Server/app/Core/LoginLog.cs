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

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.Verification;

using Ict.Petra.Shared;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MSysMan.Data.Access;


namespace Ict.Petra.Server.App.Core.Security
{
    /// <summary>
    /// Reads and saves entries in the Login Log table.
    /// </summary>
    public class TLoginLog
    {
        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AUserID"></param>
        /// <param name="ALoginStatus"></param>
        /// <param name="AProcessID"></param>
        public static void AddLoginLogEntry(String AUserID,
            String ALoginStatus,
            out Int32 AProcessID)
        {
            AddLoginLogEntry(AUserID, ALoginStatus, false, out AProcessID);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AUserID"></param>
        /// <param name="ALoginStatus"></param>
        /// <param name="AImmediateLogout"></param>
        /// <param name="AProcessID"></param>
        public static void AddLoginLogEntry(String AUserID,
            String ALoginStatus,
            Boolean AImmediateLogout,
            out Int32 AProcessID)
        {
            TDBTransaction ReadTransaction;
            TDBTransaction WriteTransaction;

            SLoginTable LoginTable;
            SLoginRow NewLoginRow;

            OdbcParameter[] ParametersArray;
            DateTime LoginDateTime;
            LoginTable = new SLoginTable();
            NewLoginRow = LoginTable.NewRowTyped(false);
            LoginDateTime = DateTime.Now;

            // Set DataRow values
            NewLoginRow.LoginProcessId = -1;
            NewLoginRow.UserId = AUserID.ToUpper();
            NewLoginRow.LoginStatus = ALoginStatus;
            NewLoginRow.LoginDate = LoginDateTime.Date;
            NewLoginRow.LoginTime = Conversions.DateTimeToInt32Time(LoginDateTime);

            if (AImmediateLogout)
            {
                NewLoginRow.LogoutDate = LoginDateTime;
                NewLoginRow.LogoutTime = Conversions.DateTimeToInt32Time(LoginDateTime);
            }

            LoginTable.Rows.Add(NewLoginRow);

            // Save DataRow
            WriteTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                // especially in the unit tests, we need to allow several logins per minute, without unique key violation
                while (SLoginAccess.Exists(NewLoginRow.UserId, NewLoginRow.LoginDate, NewLoginRow.LoginTime, WriteTransaction))
                {
                    NewLoginRow.LoginTime++;
                }

                SLoginAccess.SubmitChanges(LoginTable, WriteTransaction);

                DBAccess.GDBAccessObj.CommitTransaction();
            }
            catch (Exception Exc)
            {
                TLogging.Log("An Exception occured during the saving of the Login Log (#1):" + Environment.NewLine + Exc.ToString());

                DBAccess.GDBAccessObj.RollbackTransaction();

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
            ParametersArray[3] = new OdbcParameter("", OdbcType.VarChar, 50);
            ParametersArray[3].Value = (System.Object)(ALoginStatus);

            ReadTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                // ROWID for postgresql: see http://archives.postgresql.org/sydpug/2005-05/msg00002.php
                AProcessID =
                    Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT " + SLoginTable.GetLoginProcessIdDBName() + " FROM PUB_" +
                            TTypedDataTable.GetTableNameSQL(SLoginTable.TableId) +
                            ' ' +
                            "WHERE " + SLoginTable.GetUserIdDBName() + " = ? AND " + SLoginTable.GetLoginDateDBName() + " = ? AND " +
                            SLoginTable.GetLoginTimeDBName() + " = ? AND " + SLoginTable.GetLoginStatusDBName() + " = ?", ReadTransaction,
                            ParametersArray));

                DBAccess.GDBAccessObj.CommitTransaction();
            }
            catch (Exception Exc)
            {
                TLogging.Log("An Exception occured during the saving of the Login Log (#2):" + Environment.NewLine + Exc.ToString());

                DBAccess.GDBAccessObj.RollbackTransaction();

                throw;
            }
        }
    }
}