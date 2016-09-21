//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christiank
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
using System.Collections.Generic;
using System.Globalization;

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Common.Exceptions;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.Plugins.MSysMan;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MSysMan.Validation;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MSysMan.Data.Access;
using Ict.Petra.Shared.Security;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MSysMan.Security.UserManager;
using Ict.Petra.Server.MSysMan.Security.UserManager.WebConnectors;

namespace Ict.Petra.Server.MSysMan.Maintenance.WebConnectors
{
    /// <summary>
    /// maintain the system, eg. user management etc
    /// </summary>
    public class TMaintenanceWebConnector
    {
        private const string ACCT_ACTIVITY_GROUP_SEPARATOR = "|||";
        private const string ACCT_ACTIVITY_DATA_SEPARATOR = "~~~";

        /// <summary>
        /// this will create some default module permissions, for demo purposes
        /// </summary>
        public static string DEMOMODULEPERMISSIONS = "DEMOMODULEPERMISSIONS";

        private static readonly string StrUserChangedOtherUsersLockedState = Catalog.GetString(
            "User {0} changed the 'Locked' state of the user account of user {1}: the latter user account is now {2}.");

        private static readonly string StrUserChangedOtherUsersRetiredState = Catalog.GetString(
            "User {0} changed the 'Retired' state of the user account of user {1}: the latter user is now {2}.");

        /// <summary>
        /// Sets the password of any existing user. This Method takes into consideration how users are authenticated in
        /// this system by using an optional authentication plugin DLL.
        /// Only users with Module Permission SYSMAN can call this Method! They don't need to supply the current Password of
        /// the user with this Method overload (they usually don't know this).
        /// The new password must meet the password quality criteria!
        /// </summary>
        [RequireModulePermission("SYSMAN")]
        public static bool SetUserPassword(string AUsername,
            string ANewPassword,
            bool APasswordNeedsChanged,
            bool AUnretireIfRetired,
            out TVerificationResultCollection AVerification)
        {
            TVerificationResult VerificationResult;
            string UserAuthenticationMethod = TAppSettingsManager.GetValue("UserAuthenticationMethod", "OpenPetraDBSUser", false);

            AVerification = new TVerificationResultCollection();

            // Password quality check
            if (!TSharedSysManValidation.CheckPasswordQuality(ANewPassword, out VerificationResult))
            {
                AVerification.Add(VerificationResult);

                return false;
            }

            if (UserAuthenticationMethod == "OpenPetraDBSUser")
            {
                TDBTransaction SubmitChangesTransaction = null;
                bool SubmissionResult = false;
                TPetraPrincipal tempPrincipal;

                DBAccess.GDBAccessObj.BeginAutoTransaction(IsolationLevel.Serializable, ref SubmitChangesTransaction,
                    ref SubmissionResult,
                    delegate
                    {
                        SUserRow UserDR = TUserManagerWebConnector.LoadUser(AUsername.ToUpper(), out tempPrincipal,
                            SubmitChangesTransaction);
                        SUserTable UserTable = (SUserTable)UserDR.Table;

                        // Note: We are on purpose NOT checking here whether the new password is the same as the existing
                        // password (which would be done by calling the IsNewPasswordSameAsExistingPassword Method) because
                        // if we would do that then the SYSADMIN could try to find out what the password of a user is by
                        // seeing if (s)he would get a message that the new password must be different from the old password...!

                        SetNewPasswordHashAndSaltForExistingUser(UserDR, ANewPassword);

                        UserDR.PasswordNeedsChange = APasswordNeedsChanged;

                        // 'Unretire' the user if the user has been previously 'retired' and also 'unlock' the User Account if
                        // it has previously been locked
                        if (AUnretireIfRetired)
                        {
                            UserDR.Retired = false;
                            UserDR.AccountLocked = false;
                        }

                        AppendAccountActivity(UserDR, String.Format(
                                "The password of user {0} got changed by user {1} (the latter user has got SYSADMIN privileges).",
                                UserDR.UserId, UserInfo.GUserInfo.UserID));

                        try
                        {
                            SUserAccess.SubmitChanges(UserTable, SubmitChangesTransaction);
                        }
                        catch (Exception Exc)
                        {
                            TLogging.Log("An Exception occured during the saving of the new User Password by the SYSADMIN:" + Environment.NewLine +
                                Exc.ToString());

                            throw;
                        }

                        SubmissionResult = true;
                    });

                return true;
            }
            else
            {
                IUserAuthentication auth = TUserManagerWebConnector.LoadAuthAssembly(UserAuthenticationMethod);

                return auth.SetPassword(AUsername, ANewPassword);
            }
        }

        /// <summary>
        /// Changes the password of the *current user*. This takes into consideration how users are authenticated in
        /// this system by using an optional authentication plugin DLL.
        /// Any user can call this, but they must only change their own password, they need to know their current password,
        /// the new password must meet the password quality criteria and it must not be the same than the current password!
        /// </summary>
        [RequireModulePermission("NONE")]
        public static bool SetUserPassword(string AUserID,
            string ANewPassword,
            string ACurrentPassword,
            bool APasswordNeedsChanged,
            out TVerificationResultCollection AVerification)
        {
            TDBTransaction Transaction;
            string UserAuthenticationMethod = TAppSettingsManager.GetValue("UserAuthenticationMethod", "OpenPetraDBSUser", false);
            TVerificationResult VerificationResult;
            TVerificationResultCollection VerificationResultColl = null;
            SUserRow UserDR = null;
            SUserTable UserTable = null;

            AVerification = new TVerificationResultCollection();

            // Security check: Is the user that is performing the password change request the current user?
            if (AUserID != UserInfo.GUserInfo.UserID)
            {
                throw new EOPAppException(
                    "The setting of a User's Password must only be done by the user itself, but this isn't the case here and therefore the request gets denied");
            }

            // Password quality check
            if (!TSharedSysManValidation.CheckPasswordQuality(ANewPassword, out VerificationResult))
            {
                AVerification.Add(VerificationResult);

                return false;
            }

            if (UserAuthenticationMethod == "OpenPetraDBSUser")
            {
                TPetraPrincipal tempPrincipal;
                TDBTransaction SubmitChangesTransaction = null;
                bool SubmissionResult = false;

                DBAccess.GDBAccessObj.BeginAutoTransaction(IsolationLevel.Serializable, ref SubmitChangesTransaction,
                    ref SubmissionResult,
                    delegate
                    {
                        UserDR = TUserManagerWebConnector.LoadUser(AUserID.ToUpper(), out tempPrincipal,
                            SubmitChangesTransaction);
                        UserTable = (SUserTable)UserDR.Table;

                        // Security check: Is the supplied current password correct?
                        if (TUserManagerWebConnector.CreateHashOfPassword(ACurrentPassword,
                                UserDR.PasswordSalt) != UserDR.PasswordHash)
                        {
                            VerificationResultColl = new TVerificationResultCollection();
                            VerificationResultColl.Add(new TVerificationResult("Password Verification",
                                    String.Format(Catalog.GetString(
                                            "The current password was entered incorrectly! The password did not get changed.")),
                                    TResultSeverity.Resv_Critical));

                            AppendAccountActivity(UserDR,
                                String.Format("User {0} supplied the wrong current password while attempting to change his/her password!",
                                    UserInfo.GUserInfo.UserID));

                            try
                            {
                                SUserAccess.SubmitChanges(UserTable, SubmitChangesTransaction);

                                SubmissionResult = true;
                            }
                            catch (Exception Exc)
                            {
                                TLogging.Log(String.Format(
                                        "An Exception occured during the changing of the User Password by user '{0}' (Situation 1):", AUserID) +
                                    Environment.NewLine + Exc.ToString());

                                throw;
                            }
                        }
                    });

                if (VerificationResultColl != null)
                {
                    AVerification = VerificationResultColl;
                    return false;
                }

                // Security check: Is the supplied new password the same than the current password?
                if (IsNewPasswordSameAsExistingPassword(ANewPassword, UserDR, out VerificationResult))
                {
                    AVerification.Add(VerificationResult);

                    return false;
                }

                //
                // All checks passed: We go aheand and change the user's password!
                //

                SetNewPasswordHashAndSaltForExistingUser(UserDR, ANewPassword);

                UserDR.PasswordNeedsChange = false;

                AppendAccountActivity(UserDR, String.Format("User {0} changed his/her password{1}",
                        UserInfo.GUserInfo.UserID,
                        (APasswordNeedsChanged ? Catalog.GetString(" (enforced password change.)") : ".")));

                DBAccess.GDBAccessObj.BeginAutoTransaction(IsolationLevel.Serializable, ref SubmitChangesTransaction,
                    ref SubmissionResult,
                    delegate
                    {
                        try
                        {
                            SUserAccess.SubmitChanges(UserTable, SubmitChangesTransaction);

                            SubmissionResult = true;
                        }
                        catch (Exception Exc)
                        {
                            TLogging.Log(String.Format("An Exception occured during the changing of the User Password by user '{0}' (Situation 2):",
                                    AUserID) +
                                Environment.NewLine + Exc.ToString());

                            throw;
                        }
                    });

                return true;
            }
            else
            {
                IUserAuthentication auth = TUserManagerWebConnector.LoadAuthAssembly(UserAuthenticationMethod);

                return auth.SetPassword(AUserID, ANewPassword, ACurrentPassword);
            }
        }

        /// <summary>
        /// Compares the new password with existing password - are they the same?
        /// </summary>
        /// <param name="ANewPassword">New password.</param>
        /// <param name="AUserDR">DataRow of the user record in s_user DB Table whose password should be changed.</param>
        /// <param name="AVerificationResult">Will be null if the new password is not the same than the old password,
        /// otherwise it will be populated.</param>
        /// <returns>False if the new password is not the same than the old password, otherwise true.</returns>
        private static bool IsNewPasswordSameAsExistingPassword(string ANewPassword, SUserRow AUserDR,
            out TVerificationResult AVerificationResult)
        {
            string NewPasswordHashWithOldSalt = TUserManagerWebConnector.CreateHashOfPassword(ANewPassword,
                AUserDR.PasswordSalt, "Scrypt");

            if (PasswordHelper.EqualsAntiTimingAttack(Convert.FromBase64String(AUserDR.PasswordHash),
                    Convert.FromBase64String(NewPasswordHashWithOldSalt)))
            {
                AVerificationResult = new TVerificationResult("Password change",
                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_NEW_PASSWORD_MUST_BE_DIFFERENT));

                return true;
            }

            AVerificationResult = null;

            return false;
        }

        private static void AppendAccountActivity(SUserRow AUserDR, string AAccountActivity)
        {
            string NewAccountActivity = String.Empty;
            bool LengthOK = false;
            int PositionOfOldestActivity;

            // Prepend new Account Activity to existing FailedLoginInformation string
            NewAccountActivity +=
                DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff", CultureInfo.InvariantCulture) + ACCT_ACTIVITY_DATA_SEPARATOR + AAccountActivity;

            // If this isn't the first entry in AUserDR.FailedLoginInformation...
            if (AUserDR.AccountActivity.Length != 0)
            {
                // ...add the Separator and append the existing AUserDR.FailedLoginInformation
                NewAccountActivity += ACCT_ACTIVITY_GROUP_SEPARATOR + AUserDR.AccountActivity;
            }

            // DB column 's_failed_login_information_c' holds a maximum of 12000 characters - if we've got a longer string now
            // then we need to remove as many groups of data as needed to get below that limit to be able to save that string
            // (=oldest data gets pruned)!
            while (!LengthOK)
            {
                if (NewAccountActivity.Length >= 12000)
                {
                    PositionOfOldestActivity = NewAccountActivity.LastIndexOf(ACCT_ACTIVITY_GROUP_SEPARATOR);

                    NewAccountActivity = NewAccountActivity.Substring(0, PositionOfOldestActivity);
                }
                else
                {
                    LengthOK = true;
                }
            }

            AUserDR.AccountActivity = NewAccountActivity;

            // Also log this to the server log
            TLogging.Log(AAccountActivity);
        }

        /// <summary>
        /// creates a user, either using the default authentication with the database or with the optional authentication plugin dll
        /// </summary>
        [RequireModulePermission("SYSMAN")]
        public static bool CreateUser(string AUsername, string APassword, string AFirstName, string AFamilyName, string AModulePermissions)
        {
            TDBTransaction ReadTransaction = null;
            TDBTransaction SubmitChangesTransaction = null;
            bool UserExists = false;
            bool SubmissionOK = false;

            // TODO: check permissions. is the current user allowed to create other users?
            SUserTable userTable = new SUserTable();
            SUserRow newUser = userTable.NewRowTyped();

            newUser.UserId = AUsername;
            newUser.FirstName = AFirstName;
            newUser.LastName = AFamilyName;

            if (AUsername.Contains("@"))
            {
                newUser.EmailAddress = AUsername;
                newUser.UserId = AUsername.Substring(0, AUsername.IndexOf("@")).
                                 Replace(".", string.Empty).
                                 Replace("_", string.Empty).ToUpper();
            }

            // Check whether the user that we are asked to create already exists
            DBAccess.GDBAccessObj.BeginAutoReadTransaction(IsolationLevel.ReadCommitted, ref ReadTransaction,
                delegate
                {
                    if (SUserAccess.Exists(newUser.UserId, ReadTransaction))
                    {
                        TLogging.Log("Cannot create new user as a user with User Name '" + newUser.UserId + "' already exists!");
                        UserExists = true;
                    }
                });

            if (UserExists)
            {
                return false;
            }

            userTable.Rows.Add(newUser);

            string UserAuthenticationMethod = TAppSettingsManager.GetValue("UserAuthenticationMethod", "OpenPetraDBSUser", false);

            if (UserAuthenticationMethod == "OpenPetraDBSUser")
            {
                if (APassword.Length > 0)
                {
                    SetNewPasswordHashAndSaltForNewUser(newUser);
                }
            }
            else
            {
                try
                {
                    IUserAuthentication auth = TUserManagerWebConnector.LoadAuthAssembly(UserAuthenticationMethod);

                    if (!auth.CreateUser(AUsername, APassword, AFirstName, AFamilyName))
                    {
                        newUser = null;
                    }
                }
                catch (Exception e)
                {
                    TLogging.Log("Problem loading user authentication method " + UserAuthenticationMethod + ": " + e.ToString());
                    return false;
                }
            }

            if (newUser != null)
            {
                DBAccess.GDBAccessObj.BeginAutoTransaction(IsolationLevel.Serializable, ref SubmitChangesTransaction, ref SubmissionOK,
                    delegate
                    {
                        SUserAccess.SubmitChanges(userTable, SubmitChangesTransaction);

                        List <string>modules = new List <string>();

                        if (AModulePermissions == DEMOMODULEPERMISSIONS)
                        {
                            modules.Add("PTNRUSER");
                            modules.Add("FINANCE-1");

                            ALedgerTable theLedgers = ALedgerAccess.LoadAll(SubmitChangesTransaction);

                            foreach (ALedgerRow ledger in theLedgers.Rows)
                            {
                                modules.Add("LEDGER" + ledger.LedgerNumber.ToString("0000"));
                            }
                        }
                        else
                        {
                            string[] modulePermissions = AModulePermissions.Split(new char[] { ',' });

                            foreach (string s in modulePermissions)
                            {
                                if (s.Trim().Length > 0)
                                {
                                    modules.Add(s.Trim());
                                }
                            }
                        }

                        SUserModuleAccessPermissionTable moduleAccessPermissionTable = new SUserModuleAccessPermissionTable();

                        foreach (string module in modules)
                        {
                            SUserModuleAccessPermissionRow moduleAccessPermissionRow = moduleAccessPermissionTable.NewRowTyped();
                            moduleAccessPermissionRow.UserId = newUser.UserId;
                            moduleAccessPermissionRow.ModuleId = module;
                            moduleAccessPermissionRow.CanAccess = true;
                            moduleAccessPermissionTable.Rows.Add(moduleAccessPermissionRow);
                        }

                        SUserModuleAccessPermissionAccess.SubmitChanges(moduleAccessPermissionTable, SubmitChangesTransaction);

                        // TODO: table permissions should be set by the module list
                        // TODO: add p_data_label... tables here so user can generally have access
                        string[] tables = new string[] {
                            "p_bank", "p_church", "p_family", "p_location",
                            "p_organisation", "p_partner", "p_partner_location",
                            "p_partner_type", "p_person", "p_unit", "p_venue",
                            "p_data_label", "p_data_label_lookup", "p_data_label_lookup_category", "p_data_label_use", "p_data_label_value_partner",
                        };

                        SUserTableAccessPermissionTable tableAccessPermissionTable = new SUserTableAccessPermissionTable();

                        foreach (string table in tables)
                        {
                            SUserTableAccessPermissionRow tableAccessPermissionRow = tableAccessPermissionTable.NewRowTyped();
                            tableAccessPermissionRow.UserId = newUser.UserId;
                            tableAccessPermissionRow.TableName = table;
                            tableAccessPermissionTable.Rows.Add(tableAccessPermissionRow);
                        }

                        SUserTableAccessPermissionAccess.SubmitChanges(tableAccessPermissionTable, SubmitChangesTransaction);

                        SubmissionOK = true;
                    });

                return true;
            }

            return false;
        }

        /// <summary>
        /// check the implementation of the authentication mechanism and which functionality is implemented for OpenPetra
        /// </summary>
        /// <returns></returns>
        [RequireModulePermission("NONE")]
        public static bool GetAuthenticationFunctionality(out bool ACanCreateUser, out bool ACanChangePassword, out bool ACanChangePermissions)
        {
            ACanCreateUser = true;
            ACanChangePassword = true;
            ACanChangePermissions = true;

            string UserAuthenticationMethod = TAppSettingsManager.GetValue("UserAuthenticationMethod", "OpenPetraDBSUser", false);

            if (UserAuthenticationMethod != "OpenPetraDBSUser")
            {
                IUserAuthentication auth = TUserManagerWebConnector.LoadAuthAssembly(UserAuthenticationMethod);

                auth.GetAuthenticationFunctionality(out ACanCreateUser, out ACanChangePassword, out ACanChangePermissions);
            }

            return true;
        }

        /// <summary>
        /// load all users in the database and their permissions
        /// </summary>
        /// <returns></returns>
        [RequireModulePermission("SYSMAN")]
        public static MaintainUsersTDS LoadUsersAndModulePermissions()
        {
            TDBTransaction ReadTransaction = null;
            MaintainUsersTDS ReturnValue = new MaintainUsersTDS();

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref ReadTransaction,
                delegate
                {
                    SUserAccess.LoadAll(ReturnValue, ReadTransaction);
                    SUserModuleAccessPermissionAccess.LoadAll(ReturnValue, ReadTransaction);
                    SModuleAccess.LoadAll(ReturnValue, ReadTransaction);
                });

            return ReturnValue;
        }

        /// <summary>
        /// this is called from the MaintainUsers screen, for adding users, retiring users, set the password, etc
        /// </summary>
        [RequireModulePermission("SYSMAN")]
        public static TSubmitChangesResult SaveSUser(ref MaintainUsersTDS ASubmitDS)
        {
            TSubmitChangesResult ReturnValue = TSubmitChangesResult.scrError;

            bool CanCreateUser;
            bool CanChangePassword;
            bool CanChangePermissions;

            GetAuthenticationFunctionality(out CanCreateUser, out CanChangePassword, out CanChangePermissions);

            // make sure users are not deleted or added if this is not possible
            if (!CanCreateUser && (ASubmitDS.SUser != null))
            {
                Int32 Counter = 0;

                while (Counter < ASubmitDS.SUser.Rows.Count)
                {
                    if (ASubmitDS.SUser.Rows[Counter].RowState != DataRowState.Modified)
                    {
                        ASubmitDS.SUser.Rows.RemoveAt(Counter);
                    }
                    else
                    {
                        Counter++;
                    }
                }
            }

            if (!CanChangePermissions && (ASubmitDS.SUserModuleAccessPermission != null))
            {
                ASubmitDS.SUserModuleAccessPermission.Clear();
            }

            // TODO: if user module access permissions have changed, automatically update the table access permissions?

            if (ASubmitDS.SUser != null)
            {
                foreach (SUserRow user in ASubmitDS.SUser.Rows)
                {
                    // for new users: create users on the alternative authentication method
                    if (user.RowState == DataRowState.Added)
                    {
                        CreateUser(user.UserId, user.PasswordHash, user.FirstName, user.LastName, string.Empty);
                        user.AcceptChanges();
                    }
                    else
                    {
                        // If a password has been added for the first time there will be a (unecrypted) password and no Salt.
                        // --> Create Salt and hash.
                        if ((user.PasswordHash.Length > 0) && user.IsPasswordSaltNull())
                        {
                            SetNewPasswordHashAndSaltForNewUser(user);
                        }

                        // Has the 'Account Locked' state changed?
                        if (Convert.ToBoolean(user[SUserTable.GetAccountLockedDBName(), DataRowVersion.Original]) != user.AccountLocked)
                        {
                            if (user.AccountLocked)
                            {
                                AppendAccountActivity(user, String.Format(
                                        StrUserChangedOtherUsersLockedState, UserInfo.GUserInfo.UserID,
                                        user.UserId, Catalog.GetString("locked")));
                            }
                            else
                            {
                                AppendAccountActivity(user, String.Format(
                                        StrUserChangedOtherUsersLockedState, UserInfo.GUserInfo.UserID,
                                        user.UserId, Catalog.GetString("no longer locked")));
                            }
                        }

                        // Has the 'Account Locked' state changed?
                        if (Convert.ToBoolean(user[SUserTable.GetRetiredDBName(), DataRowVersion.Original]) != user.Retired)
                        {
                            if (user.Retired)
                            {
                                AppendAccountActivity(user, String.Format(
                                        StrUserChangedOtherUsersRetiredState, UserInfo.GUserInfo.UserID,
                                        user.UserId, Catalog.GetString("retired")));
                            }
                            else
                            {
                                AppendAccountActivity(user, String.Format(
                                        StrUserChangedOtherUsersRetiredState, UserInfo.GUserInfo.UserID,
                                        user.UserId, Catalog.GetString("no longer retired")));
                            }
                        }
                    }
                }
            }

            try
            {
                MaintainUsersTDSAccess.SubmitChanges(ASubmitDS);

                ReturnValue = TSubmitChangesResult.scrOK;
            }
            catch (Exception e)
            {
                TLogging.Log(e.Message);
                TLogging.Log(e.StackTrace);

                throw;
            }

            return ReturnValue;
        }

        private static void SetNewPasswordHashAndSaltForNewUser(SUserRow AUserRow)
        {
            bool StringsWithoutNullBytes = false;

            // PostgreSQL cannot deal with null bytes in strings, hence we need to ensure that we don't have
            // any in either the Salt or the password Hash before trying to store them to the DB (if we were
            // attempting to do that we would get NpgsqlException: 08P01, 'invalid message format' - see
            // https://github.com/npgsql/npgsql/issues/488)
            while (!StringsWithoutNullBytes)
            {
                AUserRow.PasswordSalt = PasswordHelper.GetNewPasswordSalt();
                AUserRow.PasswordHash = PasswordHelper.GetPasswordHash(AUserRow.PasswordHash, AUserRow.PasswordSalt);

                if (!AUserRow.PasswordSalt.Contains("\0")
                    && !AUserRow.PasswordHash.Contains("\0"))
                {
                    StringsWithoutNullBytes = true;
                }
                else
                {
                    TLogging.LogAtLevel(1, "PasswordSalt or PasswordSalt contained null byte(s), therefore generating " +
                        "new ones for the new user to avoid PostgreSQL problems");
                }
            }

            AUserRow.PasswordNeedsChange = true;
        }

        private static void SetNewPasswordHashAndSaltForExistingUser(SUserRow AUserRow, string ANewPassword)
        {
            bool StringsWithoutNullBytes = false;

            // PostgreSQL cannot deal with null bytes in strings, hence we need to ensure that we don't have
            // any in either the Salt or the password Hash before trying to store them to the DB (if we were
            // attempting to do that we would get NpgsqlException: 08P01, 'invalid message format' - see
            // https://github.com/npgsql/npgsql/issues/488)
            while (!StringsWithoutNullBytes)
            {
                // We always assign a new 'Salt' with every password change (best practice)!
                AUserRow.PasswordSalt = PasswordHelper.GetNewPasswordSalt();

                AUserRow.PasswordHash = TUserManagerWebConnector.CreateHashOfPassword(ANewPassword,
                    AUserRow.PasswordSalt, "Scrypt");

                if (!AUserRow.PasswordSalt.Contains("\0")
                    && !AUserRow.PasswordHash.Contains("\0"))
                {
                    StringsWithoutNullBytes = true;
                }
                else
                {
                    TLogging.LogAtLevel(1, "PasswordSalt or PasswordSalt contained null byte(s), therefore generating " +
                        "new ones for the same password to avoid PostgreSQL problems");
                }
            }
        }
    }
}
