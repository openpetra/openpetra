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

using Ict.Common;
using Ict.Common.DB;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MSysMan.Data.Access;

namespace Ict.Petra.Server.App.Core.Security
{
    /// <summary>
    /// Saves entries in the 'User Account Activity Log' table (s_user_account_activity).
    /// </summary>
    public class TUserAccountActivityLog
    {
        /// <summary>User record got created (usually by a system administrator).</summary>
        public const string USER_ACTIVITY_USER_RECORD_CREATED = "USER_RECORD_CREATED";
        /// <summary>User's password got changed by a system administrator.</summary>
        public const string USER_ACTIVITY_PWD_CHANGE_BY_SYSADMIN = "PWD_CHANGE_BY_SYSADMIN";
        /// <summary>A system administrator made an attempt to change a User's password for a UserID that doesn't exist.</summary>
        public const string USER_ACTIVITY_PWD_CHANGE_ATTEMPT_BY_SYSADMIN_FOR_NONEXISTING_USER = "PWD_CHANGE_ATTEMPT_BY_SYSADMIN_FOR_NONEXISTING_USER";
        /// <summary>User's password got changed by the user (not triggered by an enforced password change).</summary>
        public const string USER_ACTIVITY_PWD_CHANGE_BY_USER = "PWD_CHANGE_BY_USER";
        /// <summary>User's password got changed by the user (triggered by an enforced password change).</summary>
        public const string USER_ACTIVITY_PWD_CHANGE_BY_USER_ENFORCED = "PWD_CHANGE_BY_USER_ENFORCED";
        /// <summary>A user tried to make an attempt to change a User's password for a UserID that doesn't exist.</summary>
        public const string USER_ACTIVITY_PWD_CHANGE_ATTEMPT_BY_USER_FOR_NONEXISTING_USER = "PWD_CHANGE_ATTEMPT_BY_USER_FOR_NONEXISTING_USER";
        /// <summary>User supplied the wrong current password while attempting to change his/her password.</summary>
        public const string USER_ACTIVITY_PWD_WRONG_WHILE_PWD_CHANGE = "PWD_WRONG_WHILE_PWD_CHANGE";
        /// <summary>Permitted number of failed logins in a row got exceeded and the user account for the user
        /// got locked because of this!</summary>
        public const string USER_ACTIVITY_PERMITTED_FAILED_LOGINS_EXCEEDED = "PERMITTED_FAILED_LOGINS_EXCEEDED";
        /// <summary>Password Hashing Scheme Version of the user got upgraded (from Version to Version).</summary>
        public const string USER_ACTIVITY_PWD_HASHING_SCHEME_UPGRADED = "PWD_HASHING_SCHEME_UPGRADED";
        /// <summary>'Retired' state of the user account got changed to 'Retired' by a system administrator.</summary>
        public const string USER_ACTIVITY_USER_GOT_RETIRED = "USER_GOT_RETIRED";
        /// <summary>'Retired' state of the user account got changed to 'Not Retired' by a system administrator.</summary>
        public const string USER_ACTIVITY_USER_GOT_UNRETIRED = "USER_GOT_UNRETIRED";
        /// <summary>'Locked' state of the user account got changed to 'Locked', either by a system administrator
        /// or automatically because the permitted number of failed logins in a row got exceeded - see also
        /// <see cref="USER_ACTIVITY_PERMITTED_FAILED_LOGINS_EXCEEDED"/>.</summary>
        public const string USER_ACTIVITY_USER_ACCOUNT_GOT_LOCKED = "USER_ACCOUNT_GOT_LOCKED";
        /// <summary>'Locked' state of the user account got changed to 'Unlocked' by a system administrator.</summary>
        public const string USER_ACTIVITY_USER_ACCOUNT_GOT_UNLOCKED = "USER_ACCOUNT_GOT_UNLOCKED";

        /// <summary>
        /// Adds an entry to the 'User Account Activity Log' table (s_user_account_activity).
        /// </summary>
        /// <param name="AUserID">UserID of the user to add the User Account Activity for.</param>
        /// <param name="AActivityType">Type of the User Account Activity.</param>
        /// <param name="AActivityDetails">Details/description of the User Account Activity.</param>
        /// <param name="ATransaction">Instantiated DB Transaction.</param>
        public static void AddUserAccountActivityLogEntry(String AUserID,
            String AActivityType,
            String AActivityDetails,
            TDBTransaction ATransaction)
        {
            SUserAccountActivityTable ActivityTable = new SUserAccountActivityTable();
            SUserAccountActivityRow NewActivityRow = ActivityTable.NewRowTyped(false);
            DateTime ActivityDateTime = DateTime.Now;

            // Set DataRow values
            NewActivityRow.UserId = AUserID.ToUpper();
            NewActivityRow.ActivityDate = ActivityDateTime.Date;
            NewActivityRow.ActivityTime = Conversions.DateTimeToInt32Time(ActivityDateTime);
            NewActivityRow.ActivityType = AActivityType;
            NewActivityRow.ActivityDetails = AActivityDetails;

            ActivityTable.Rows.Add(NewActivityRow);

            try
            {
                // We need to allow several user account activity log entries per minute, without unique key violation
                while (SLoginAccess.Exists(NewActivityRow.UserId, NewActivityRow.ActivityDate, NewActivityRow.ActivityTime,
                           ATransaction))
                {
                    NewActivityRow.ActivityTime++;
                }

                SUserAccountActivityAccess.SubmitChanges(ActivityTable, ATransaction);

                // Also log this to the server log
                TLogging.Log(AActivityDetails);
            }
            catch (Exception Exc)
            {
                TLogging.Log("An Exception occured during the saving of a User Account Activity Log entry:" +
                    Environment.NewLine + Exc.ToString());

                throw;
            }
        }
    }
}