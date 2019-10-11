//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christiank
//
// Copyright 2004-2019 by OM International
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
using System.Collections.Generic;
using System.Globalization;

using Newtonsoft.Json;

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Common.Exceptions;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.Plugins.MSysMan;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MSysMan.Validation;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Server.MSysMan.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.Security;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MSysMan.Security;
using Ict.Petra.Server.MSysMan.Security.UserManager.WebConnectors;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Server.MSysMan.Maintenance.WebConnectors
{
    /// <summary>
    /// maintain the system, eg. user management etc
    /// </summary>
    public class TMaintenanceWebConnector
    {
        /// <summary>
        /// this will create some default module permissions, for demo purposes
        /// </summary>
        public static readonly string DEMOMODULEPERMISSIONS = "DEMOMODULEPERMISSIONS";

        private static readonly string StrUserChangedOtherUsersLockedState = Catalog.GetString(
            "User {0} changed the 'Locked' state of the user account of user {1}: the latter user account is now {2}. ");

        private static readonly string StrUserChangedOtherUsersRetiredState = Catalog.GetString(
            "User {0} changed the 'Retired' state of the user account of user {1}: the latter user is now {2}. ");

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
            string AClientComputerName, string AClientIPAddress,
            out TVerificationResultCollection AVerification)
        {
            TVerificationResult VerificationResult;
            SUserTable UserTable;
            SUserRow UserDR;
            string UserAuthenticationMethod = TAppSettingsManager.GetValue("UserAuthenticationMethod", "OpenPetraDBSUser", false);
            bool BogusPasswordChangeAttempt = false;

            AVerification = new TVerificationResultCollection();

            // Password quality check
            if (!TSharedSysManValidation.CheckPasswordQuality(ANewPassword, out VerificationResult))
            {
                AVerification.Add(VerificationResult);

                return false;
            }

            if (UserAuthenticationMethod == "OpenPetraDBSUser")
            {
                TDBTransaction SubmitChangesTransaction = new TDBTransaction();
                bool SubmissionResult = false;
                TPetraPrincipal tempPrincipal;

                DBAccess.WriteTransaction(ref SubmitChangesTransaction,
                    ref SubmissionResult,
                    delegate
                    {
                        try
                        {
                            UserDR = TUserManagerWebConnector.LoadUser(AUsername.ToUpper(), out tempPrincipal,
                                SubmitChangesTransaction);
                        }
                        catch (EUserNotExistantException)
                        {
                            // Because this cannot happen when a password change gets effected through normal OpenPetra
                            // operation this is treated as a bogus operation that an attacker launches!

                            BogusPasswordChangeAttempt = true;

                            // Logging
                            TUserAccountActivityLog.AddUserAccountActivityLogEntry(AUsername,
                                TUserAccountActivityLog.USER_ACTIVITY_PWD_CHANGE_ATTEMPT_BY_SYSADMIN_FOR_NONEXISTING_USER,
                                String.Format(Catalog.GetString(
                                        "A system administrator, {0}, made an attempt to change a User's password for UserID {1} " +
                                        "but that user doesn't exist! "), UserInfo.GetUserInfo().UserID, AUsername) +
                                String.Format(ResourceTexts.StrRequestCallerInfo, AClientComputerName, AClientIPAddress),
                                SubmitChangesTransaction);

                            SubmissionResult = true; // Need to set this so that the DB Transaction gets committed!

                            // Simulate that time is spent on 'authenticating' a user (although the user doesn't exist)...! Reason for that: see Method
                            // SimulatePasswordAuthenticationForNonExistingUser!
                            TUserManagerWebConnector.SimulatePasswordAuthenticationForNonExistingUser();

                            return;
                        }

                        UserTable = (SUserTable)UserDR.Table;

                        // Note: We are on purpose NOT checking here whether the new password is the same as the existing
                        // password (which would be done by calling the IsNewPasswordSameAsExistingPassword Method) because
                        // if we would do that then the SYSADMIN could try to find out what the password of a user is by
                        // seeing if (s)he would get a message that the new password must be different from the old password...!

                        SetNewPasswordHashAndSaltForUser(UserDR, ANewPassword, AClientComputerName, AClientIPAddress, SubmitChangesTransaction);

                        UserDR.PasswordNeedsChange = APasswordNeedsChanged;

                        // 'Unretire' the user if the user has been previously 'retired' and also 'unlock' the User Account if
                        // it has previously been locked
                        if (AUnretireIfRetired)
                        {
                            UserDR.Retired = false;
                            UserDR.AccountLocked = false;
                        }

                        try
                        {
                            SUserAccess.SubmitChanges(UserTable, SubmitChangesTransaction);

                            TUserAccountActivityLog.AddUserAccountActivityLogEntry(UserDR.UserId,
                                TUserAccountActivityLog.USER_ACTIVITY_PWD_CHANGE_BY_SYSADMIN,
                                String.Format(Catalog.GetString(
                                        "The password of user {0} got changed by user {1} (the latter user has got SYSADMIN " +
                                        "privileges). "), UserDR.UserId, UserInfo.GetUserInfo().UserID) +
                                String.Format(ResourceTexts.StrRequestCallerInfo, AClientComputerName, AClientIPAddress),
                                SubmitChangesTransaction);
                        }
                        catch (Exception Exc)
                        {
                            TLogging.Log("An Exception occured during the saving of the new User Password by the SYSADMIN:" + Environment.NewLine +
                                Exc.ToString());

                            throw;
                        }

                        SubmissionResult = true;
                    });


                return !BogusPasswordChangeAttempt;
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
        [RequireModulePermission("USER")]
        public static bool SetUserPassword(string AUserID,
            string ANewPassword,
            string ACurrentPassword,
            bool APasswordNeedsChanged,
            string AClientComputerName, string AClientIPAddress,
            out TVerificationResultCollection AVerification)
        {
            string UserAuthenticationMethod = TAppSettingsManager.GetValue("UserAuthenticationMethod", "OpenPetraDBSUser", false);
            TVerificationResult VerificationResult;
            TVerificationResultCollection VerificationResultColl = null;
            SUserRow UserDR = null;
            SUserTable UserTable = null;
            bool BogusPasswordChangeAttempt = false;

            AVerification = new TVerificationResultCollection();

            // Security check: Is the user that is performing the password change request the current user?
            AUserID = AUserID.ToUpper();
            if (AUserID != UserInfo.GetUserInfo().UserID)
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
                TDBTransaction SubmitChangesTransaction = new TDBTransaction();
                bool SubmissionResult = false;

                DBAccess.WriteTransaction(ref SubmitChangesTransaction,
                    ref SubmissionResult,
                    delegate
                    {
                        try
                        {
                            UserDR = TUserManagerWebConnector.LoadUser(AUserID.ToUpper(), out tempPrincipal,
                                SubmitChangesTransaction);
                        }
                        catch (EUserNotExistantException)
                        {
                            // Because this cannot happen when a password change gets effected through normal OpenPetra
                            // operation this is treated as a bogus operation that an attacker launches!

                            BogusPasswordChangeAttempt = true;

                            // Logging
                            TUserAccountActivityLog.AddUserAccountActivityLogEntry(AUserID,
                                TUserAccountActivityLog.USER_ACTIVITY_PWD_CHANGE_ATTEMPT_BY_USER_FOR_NONEXISTING_USER,
                                String.Format(Catalog.GetString(
                                        "User {0} tried to make an attempt to change a User's password for UserID {1} " +
                                        "but that user doesn't exist! "), UserInfo.GetUserInfo().UserID, AUserID) +
                                String.Format(ResourceTexts.StrRequestCallerInfo, AClientComputerName, AClientIPAddress),
                                SubmitChangesTransaction);

                            SubmissionResult = true; // Need to set this so that the DB Transaction gets committed!

                            // Simulate that time is spent on 'authenticating' a user (although the user doesn't exist)...! Reason for that: see Method
                            // SimulatePasswordAuthenticationForNonExistingUser!
                            TUserManagerWebConnector.SimulatePasswordAuthenticationForNonExistingUser();

                            return;
                        }

                        UserTable = (SUserTable)UserDR.Table;

                        // Security check: Is the supplied current password correct?
                        if (TUserManagerWebConnector.CreateHashOfPassword(ACurrentPassword,
                                UserDR.PasswordSalt, UserDR.PwdSchemeVersion) != UserDR.PasswordHash)
                        {
                            VerificationResultColl = new TVerificationResultCollection();
                            VerificationResultColl.Add(new TVerificationResult("Password Verification",
                                    Catalog.GetString(
                                        "The current password was entered incorrectly! The password did not get changed."),
                                    "ChangePassword.CurrentPwdWrong",
                                    TResultSeverity.Resv_Critical));

                            try
                            {
                                SUserAccess.SubmitChanges(UserTable, SubmitChangesTransaction);

                                TUserAccountActivityLog.AddUserAccountActivityLogEntry(UserDR.UserId,
                                    TUserAccountActivityLog.USER_ACTIVITY_PWD_WRONG_WHILE_PWD_CHANGE,
                                    String.Format(Catalog.GetString(
                                            "User {0} supplied the wrong current password while attempting to change " +
                                            "his/her password! ") +
                                        String.Format(ResourceTexts.StrRequestCallerInfo, AClientComputerName, AClientIPAddress),
                                        UserInfo.GetUserInfo().UserID),
                                    SubmitChangesTransaction);

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

                if (BogusPasswordChangeAttempt)
                {
                    // Note: VerificationResultColl will be null in this case because we don't want to disclose to an attackeer
                    // why the password change attempt was denied!!!
                    return false;
                }

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

                SetNewPasswordHashAndSaltForUser(UserDR, ANewPassword, AClientComputerName, AClientIPAddress, SubmitChangesTransaction);

                UserDR.PasswordNeedsChange = false;

                DBAccess.WriteTransaction(ref SubmitChangesTransaction,
                    ref SubmissionResult,
                    delegate
                    {
                        try
                        {
                            SUserAccess.SubmitChanges(UserTable, SubmitChangesTransaction);

                            TUserAccountActivityLog.AddUserAccountActivityLogEntry(UserDR.UserId,
                                (APasswordNeedsChanged ? TUserAccountActivityLog.USER_ACTIVITY_PWD_CHANGE_BY_USER_ENFORCED :
                                 TUserAccountActivityLog.USER_ACTIVITY_PWD_CHANGE_BY_USER),
                                String.Format(Catalog.GetString("User {0} changed his/her password{1}"),
                                    UserInfo.GetUserInfo().UserID,
                                    (APasswordNeedsChanged ? Catalog.GetString(" (enforced password change.) ") : ". ")) +
                                String.Format(ResourceTexts.StrRequestCallerInfo, AClientComputerName, AClientIPAddress),
                                SubmitChangesTransaction);

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

        /// lock the user SYSADMIN while this instance is not assigned to a customer yet
        [NoRemoting]
        public static bool LockSysadmin()
        {
            TDataBase db = DBAccess.Connect("LockSysadmin");
            TDBTransaction Transaction = new TDBTransaction();
            bool SubmissionOK = true;

            db.WriteTransaction(ref Transaction, ref SubmissionOK,
                delegate
                {
                    string sqlUpdate = "UPDATE s_user SET s_account_locked_l = 1, s_retired_l = 1, s_password_hash_c = 'invalid' WHERE s_user_id_c = 'SYSADMIN'";
                    db.ExecuteNonQuery(sqlUpdate, Transaction);
                });

            db.CloseDBConnection();

            return true;
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
                AUserDR.PasswordSalt, AUserDR.PwdSchemeVersion);

            if (TPasswordHelper.EqualsAntiTimingAttack(Convert.FromBase64String(AUserDR.PasswordHash),
                    Convert.FromBase64String(NewPasswordHashWithOldSalt)))
            {
                AVerificationResult = new TVerificationResult("Password change",
                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_NEW_PASSWORD_MUST_BE_DIFFERENT));

                return true;
            }

            AVerificationResult = null;

            return false;
        }

        /// <summary>
        /// creates a user, either using the default authentication with the database or with the optional authentication plugin dll
        /// </summary>
        [NoRemoting]
        public static bool CreateUser(string AUsername, string APassword, string AFirstName, string AFamilyName, string ALanguageCode,
            string AModulePermissions,
            string AToken, out string AUserID, TDataBase ADataBase = null)
        {
            TDataBase DBConnectionObj = DBAccess.Connect("CreateUser", ADataBase);
            TDBTransaction ReadWriteTransaction = new TDBTransaction();
            bool NewTransaction;
            bool SubmissionOK = false;

            // TODO: check permissions. is the current user allowed to create other users?
            SUserTable userTable = new SUserTable();
            SUserRow newUser = userTable.NewRowTyped();

            newUser.UserId = AUsername;
            newUser.FirstName = AFirstName;
            newUser.LastName = AFamilyName;
            newUser.LanguageCode = ALanguageCode;
            
            if (AToken != String.Empty)
            {
                newUser.PasswordResetToken = AToken;
                newUser.AccountLocked = true;
            }

            if (AUsername.Contains("@"))
            {
                newUser.EmailAddress = AUsername;
                newUser.UserId = AUsername.Substring(0, AUsername.IndexOf("@")).
                                 Replace(".", string.Empty).
                                 Replace("_", string.Empty).ToUpper();
            }

            AUserID = newUser.UserId;

            ReadWriteTransaction = DBConnectionObj.GetNewOrExistingTransaction(
                IsolationLevel.Serializable, out NewTransaction, "CreateUser");

            try
            {
                // Check whether the user that we are asked to create already exists
                if (SUserAccess.Exists(newUser.UserId, ReadWriteTransaction))
                {
                    TLogging.Log("Cannot create new user because a user with User Name '" + newUser.UserId + "' already exists!");

                    return false;
                }

                newUser.PwdSchemeVersion = TPasswordHelper.CurrentPasswordSchemeNumber;

                userTable.Rows.Add(newUser);

                string UserAuthenticationMethod = TAppSettingsManager.GetValue("UserAuthenticationMethod", "OpenPetraDBSUser", false);

                if (UserAuthenticationMethod == "OpenPetraDBSUser")
                {
                    if (APassword.Length > 0)
                    {
                        SetNewPasswordHashAndSaltForUser(newUser, APassword, string.Empty, string.Empty, ReadWriteTransaction);

                        if (AModulePermissions != TMaintenanceWebConnector.DEMOMODULEPERMISSIONS)
                        {
                            newUser.PasswordNeedsChange = true;
                        }
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
                    SUserAccess.SubmitChanges(userTable, ReadWriteTransaction);

                    List <string>modules = new List <string>();

                    if (AModulePermissions == DEMOMODULEPERMISSIONS)
                    {
                        modules.Add("PTNRUSER");
                        modules.Add("FINANCE-1");

                        ALedgerTable theLedgers = ALedgerAccess.LoadAll(ReadWriteTransaction);

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

                    SUserModuleAccessPermissionAccess.SubmitChanges(moduleAccessPermissionTable, ReadWriteTransaction);

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

                    SUserTableAccessPermissionAccess.SubmitChanges(tableAccessPermissionTable, ReadWriteTransaction);

                    TUserAccountActivityLog.AddUserAccountActivityLogEntry(newUser.UserId,
                        TUserAccountActivityLog.USER_ACTIVITY_USER_RECORD_CREATED,
                        String.Format(Catalog.GetString("The user record for the new user {0} got created by user {1}. "),
                            newUser.UserId, UserInfo.GetUserInfo().UserID) +
                        String.Format(ResourceTexts.StrRequestCallerInfo, string.Empty, string.Empty),
                        ReadWriteTransaction);

                    SubmissionOK = true;

                    return true;
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    if (SubmissionOK)
                    {
                        ReadWriteTransaction.Commit();
                    }
                    else
                    {
                        ReadWriteTransaction.Rollback();
                    }
                }
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
            TDBTransaction ReadTransaction = new TDBTransaction();
            MaintainUsersTDS ReturnValue = new MaintainUsersTDS();

            DBAccess.ReadTransaction(
                ref ReadTransaction,
                delegate
                {
                    SUserAccess.LoadAll(ReturnValue, ReadTransaction);
                    SUserModuleAccessPermissionAccess.LoadAll(ReturnValue, ReadTransaction);
                    SModuleAccess.LoadAll(ReturnValue, ReadTransaction);
                });

            // Remove Password Hash and Password 'Salt' before passing it out to the caller - these aren't needed
            // and it is better to not hand them out needlessly to prevent possible 'eavesdropping' by an attacker
            // (who could otherwise gather the Password Hashes and Password 'Salts' of all users in one go if the
            // attacker manages to listen to network traffic). (#5502)!
            foreach (var UserRow in ReturnValue.SUser.Rows)
            {
                ((SUserRow)UserRow).PasswordHash = "***";
                ((SUserRow)UserRow).PasswordSalt = "***";
            }

            ReturnValue.AcceptChanges();

            return ReturnValue;
        }

        /// <summary>
        /// create new user
        /// </summary>
        [RequireModulePermission("SYSMAN")]
        public static MaintainUsersTDS CreateUserWithInitialPermissions()
        {
            TDBTransaction ReadTransaction = new TDBTransaction();
            MaintainUsersTDS ReturnValue = new MaintainUsersTDS();

            DBAccess.ReadTransaction(
                ref ReadTransaction,
                delegate
                {
                    SUserRow newUser = ReturnValue.SUser.NewRowTyped();
                    newUser.UserId = "NEWUSER";
                    ReturnValue.SUser.Rows.Add(newUser);
                    SUserModuleAccessPermissionRow newPermission = ReturnValue.SUserModuleAccessPermission.NewRowTyped();
                    newPermission.UserId = "NEWUSER";
                    newPermission.ModuleId = SharedConstants.PETRAMODULE_PTNRUSER;
                    newPermission.CanAccess = true;
                    ReturnValue.SUserModuleAccessPermission.Rows.Add(newPermission);
                    newPermission = ReturnValue.SUserModuleAccessPermission.NewRowTyped();
                    newPermission.UserId = "NEWUSER";
                    newPermission.ModuleId = SharedConstants.PETRAMODULE_FINANCE1;
                    newPermission.CanAccess = true;
                    ReturnValue.SUserModuleAccessPermission.Rows.Add(newPermission);
                    newPermission = ReturnValue.SUserModuleAccessPermission.NewRowTyped();
                    newPermission.UserId = "NEWUSER";
                    newPermission.ModuleId = SharedConstants.PETRAMODULE_FINANCERPT;
                    newPermission.CanAccess = true;
                    ReturnValue.SUserModuleAccessPermission.Rows.Add(newPermission);
                    SModuleAccess.LoadAll(ReturnValue, ReadTransaction);
                });

            ReturnValue.AcceptChanges();

            return ReturnValue;
        }

        /// <summary>
        /// load one user from the database and the permissions of that user
        /// </summary>
        [RequireModulePermission("SYSMAN")]
        public static MaintainUsersTDS LoadUserAndModulePermissions(string AUserId)
        {
            TDBTransaction ReadTransaction = new TDBTransaction();
            MaintainUsersTDS ReturnValue = new MaintainUsersTDS();

            DBAccess.ReadTransaction(
                ref ReadTransaction,
                delegate
                {
                    SUserAccess.LoadByPrimaryKey(ReturnValue, AUserId, ReadTransaction);
                    SUserModuleAccessPermissionAccess.LoadViaSUser(ReturnValue,AUserId, ReadTransaction);
                    SModuleAccess.LoadAll(ReturnValue, ReadTransaction);
                });

            // Remove Password Hash and Password 'Salt' before passing it out to the caller - these aren't needed
            // and it is better to not hand them out needlessly to prevent possible 'eavesdropping' by an attacker
            // (who could otherwise gather the Password Hashes and Password 'Salts' of all users in one go if the
            // attacker manages to listen to network traffic). (#5502)!
            foreach (var UserRow in ReturnValue.SUser.Rows)
            {
                ((SUserRow)UserRow).PasswordHash = "***";
                ((SUserRow)UserRow).PasswordSalt = "***";
            }

            ReturnValue.AcceptChanges();

            return ReturnValue;
        }

        /// <summary>
        /// save the details and permissions of one user
        /// </summary>
        [RequireModulePermission("SYSMAN")]
        public static string SaveUserAndModulePermissions(string AUserId,
            string AFirstName, string ALastName, string AEmailAddress, string ALanguageCode,
            bool AAccountLocked, bool ARetired,
            bool ASendMail,
            List<string> AModulePermissions, Int64 AModificationId)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            AUserId = AUserId.ToUpper().Trim();
            AEmailAddress = AEmailAddress.Trim();

            if (AUserId == String.Empty)
            {
                result.Add("resultcode", "error");
                result.Add("errormsg", "EMPTYUSERID");

                return JsonConvert.SerializeObject(result);
            }

            MaintainUsersTDS SubmitDS = LoadUserAndModulePermissions(AUserId);

            bool NewUser = false;
            string NewPassword = String.Empty;

            // we don't want to overwrite existing records with a new record
            // we don't want to save changes if someone else already modified the user since we got the data
            if (SubmitDS.SUser.Count == 0 && AModificationId == 0)
            {
                if (AEmailAddress == String.Empty)
                {
                    result.Add("resultcode", "error");
                    result.Add("errormsg", "EMPTYEMAILADDRESS");

                    return JsonConvert.SerializeObject(result);
                }

                if (null != TStringChecks.ValidateEmail(AEmailAddress))
                {
                    result.Add("resultcode", "error");
                    result.Add("errormsg", "INVALIDEMAILADDRESS");

                    return JsonConvert.SerializeObject(result);
                }

                if (ALanguageCode == String.Empty)
                {
                    ALanguageCode = "99";
                }

                SUserRow newUser = SubmitDS.SUser.NewRowTyped();
                newUser.UserId = AUserId;
                // user must change password on first login
                newUser.PasswordNeedsChange = true;
                // set random password
                NewPassword = TPasswordHelper.GetRandomSecurePassword();
                SetNewPasswordHashAndSaltForNewUser(newUser, NewPassword);
                SubmitDS.SUser.Rows.Add(newUser);
                NewUser = true;
            }
            else
            {
                DateTimeOffset offset = new DateTimeOffset(SubmitDS.SUser[0].ModificationId);
                if (offset.ToUnixTimeMilliseconds() != AModificationId) {
                    result.Add("resultcode", "error");
                    if (AModificationId == 0)
                    {
                        result.Add("errormsg", "USERIDALREADYEXISTS");
                    }
                    else
                    {
                        result.Add("errormsg", "MIDAIRCOLLISION");
                    }

                    return JsonConvert.SerializeObject(result);
                }
            }

            SubmitDS.SUser[0].EmailAddress = AEmailAddress;
            SubmitDS.SUser[0].FirstName = AFirstName;
            SubmitDS.SUser[0].LastName = ALastName;
            SubmitDS.SUser[0].LanguageCode = ALanguageCode;
            SubmitDS.SUser[0].AccountLocked = AAccountLocked;
            SubmitDS.SUser[0].Retired = ARetired;

            List<string> ExistingPermissions = new List<string>();
            foreach (SUserModuleAccessPermissionRow permission in SubmitDS.SUserModuleAccessPermission.Rows)
            {
                if (AModulePermissions.Contains(permission.ModuleId))
                {
                    permission.CanAccess = true;
                }
                else
                {
                    permission.CanAccess = false;
                }
                ExistingPermissions.Add(permission.ModuleId);
            }

            // add new permissions
            foreach (string module in AModulePermissions)
            {
                if (!ExistingPermissions.Contains(module))
                {
                    SUserModuleAccessPermissionRow moduleAccessPermissionRow = SubmitDS.SUserModuleAccessPermission.NewRowTyped();
                    moduleAccessPermissionRow.UserId = AUserId;
                    moduleAccessPermissionRow.ModuleId = module;
                    moduleAccessPermissionRow.CanAccess = true;
                    SubmitDS.SUserModuleAccessPermission.Rows.Add(moduleAccessPermissionRow);
                }
            }

            TSubmitChangesResult submitresult = SaveSUser(ref SubmitDS, "Web", "0.0.0.0");

            if (submitresult == TSubmitChangesResult.scrOK)
            {
                result.Add("resultcode", "success");

                if (NewUser && TAppSettingsManager.GetValue("SmtpHost").EndsWith(TSmtpSender.SMTP_HOST_DEFAULT))
                {
                    TLogging.Log("There is no configuration for SmtpHost. The new password for " + AUserId + " is " + NewPassword);
                }
                else if (NewUser && ASendMail)
                {
                    TDBTransaction SubmitChangesTransaction = new TDBTransaction();
                    bool SubmissionResult = false;

                    DBAccess.WriteTransaction(ref SubmitChangesTransaction,
                        ref SubmissionResult,
                        delegate
                        {
                            // send E-Mail with link to reset the password
                            // TODO: use double opt-in before sending the actual information???
                            string token = SaveNewToken(AUserId, SubmitChangesTransaction.DataBaseObj, SubmitChangesTransaction);

                            string Domain = TAppSettingsManager.GetValue("Server.Url");
                            string EMailDomain = TAppSettingsManager.GetValue("Server.EmailDomain");
                            Dictionary<string, string> parameters = new Dictionary<string, string>();
                            parameters.Add("UserId", AUserId);
                            parameters.Add("FirstName", SubmitDS.SUser[0].FirstName);
                            parameters.Add("LastName", SubmitDS.SUser[0].LastName);
                            parameters.Add("Domain", Domain);
                            parameters.Add("Token", token);

                            new TSmtpSender().SendEmailFromTemplate(
                                "no-reply@" + EMailDomain,
                                "OpenPetra Admin",
                                SubmitDS.SUser[0].EmailAddress,
                                "requestnewpassword",
                                ALanguageCode,
                                parameters);

                            SubmissionResult = true;
                        });
                    
                }
            }
            else
            {
                result.Add("resultcode", "error");
            }

            return JsonConvert.SerializeObject(result); 
        }

        /// <summary>
        /// set initial email address for user SYSADMIN
        /// </summary>
        [NoRemoting]
        public static bool SetInitialSysadminEmail(string AEmailAddress, string AFirstName, string ALastName, string ALanguageCode)
        {
            TDataBase db = DBAccess.Connect("InitialSysadminEmail");
            TDBTransaction Transaction = db.BeginTransaction(IsolationLevel.Serializable, 0, "InitialSysadminEmail");

            try
            {
                // check if the SYSADMIN user does not have an email address yet
                string sql = "SELECT s_email_address_c" +
                    " FROM PUB_s_user WHERE s_user_id_c = 'SYSADMIN' AND s_email_address_c <> ''";

                DataTable result = db.SelectDT(sql, "user", Transaction);

                if (result.Rows.Count > 0)
                {
                    // this instance has already been initialised
                    TLogging.Log("SetInitialSysadminEmail: has already been initialised");
                    return false;
                }

                string sqlUpdate = "UPDATE s_user SET s_account_locked_l = 0, s_retired_l = 0, s_password_hash_c = 'invalid', s_email_address_c = ?, s_first_name_c = ?, s_last_name_c = ?, s_language_code_c = ? WHERE s_user_id_c = 'SYSADMIN'";
                OdbcParameter[] parameters = new OdbcParameter[4];
                parameters[0] = new OdbcParameter("EmailAddress", OdbcType.VarChar);
                parameters[0].Value = AEmailAddress;
                parameters[1] = new OdbcParameter("FirstName", OdbcType.VarChar);
                parameters[1].Value = AFirstName;
                parameters[2] = new OdbcParameter("LastName", OdbcType.VarChar);
                parameters[2].Value = ALastName;
                parameters[3] = new OdbcParameter("LanguageCode", OdbcType.VarChar);
                parameters[3].Value = ALanguageCode;
                db.ExecuteNonQuery(sqlUpdate, Transaction, parameters, true);

                return true;
            }
            catch (Exception e)
            {
                TLogging.Log("SetInitialSysadminEmail " + e.ToString());
                return false;
            }
            finally
            {
                db.CloseDBConnection();
            }
        }

        /// generate token and save with valid date until in the s_user record
        private static string SaveNewToken(string AUserID, TDataBase db, TDBTransaction Transaction)
        {
            string token = TPasswordHelper.GetRandomToken();
            DateTime validUntil = DateTime.Now;
            validUntil = validUntil.AddHours(24);
            string sqlUpdate = "UPDATE PUB_s_user SET s_password_reset_token_c = ?, s_password_reset_valid_until_d = ?" +
                " WHERE s_user_id_c = ?";
            OdbcParameter[] parameters = new OdbcParameter[3];
            parameters[0] = new OdbcParameter("Token", OdbcType.VarChar);
            parameters[0].Value = token;
            parameters[1] = new OdbcParameter("ValidUntil", OdbcType.DateTime);
            parameters[1].Value = validUntil;
            parameters[2] = new OdbcParameter("UserID", OdbcType.VarChar);
            parameters[2].Value = AUserID;
            db.ExecuteNonQuery(sqlUpdate, Transaction, parameters, true);

            return token;
        }

        /// <summary>
        /// request new password
        /// </summary>
        [NoRemoting]
        public static bool RequestNewPassword(string AEmailAddress)
        {
            TDataBase db = DBAccess.Connect("RequestNewPassword");
            TDBTransaction Transaction = db.BeginTransaction(IsolationLevel.Serializable, 0, "RequireNewPassword");

            try
            {
                // check if we have a unique s_user with that email address
                string sql = "SELECT s_user_id_c, s_first_name_c, s_last_name_c, s_language_code_c" +
                    " FROM PUB_s_user WHERE UPPER(s_email_address_c) = ?";

                OdbcParameter[] parameters = new OdbcParameter[1];
                parameters[0] = new OdbcParameter("EmailAddress", OdbcType.VarChar);
                parameters[0].Value = AEmailAddress.ToUpper();

                DataTable result = db.SelectDT(sql, "user", Transaction, parameters);

                if (result.Rows.Count == 0)
                {
                    // not telling the requester that this email does not exist. just return true
                    return true;
                }
                if (result.Rows.Count > 1)
                {
                    TLogging.Log("RequestNewPassword: there are multiple users for e-mail " + AEmailAddress + "; no email has been sent");
                    return true;
                }
                else if (result.Rows.Count == 1)
                {
                    string UserID = result.Rows[0][0].ToString();
                    string FirstName = result.Rows[0][1].ToString();
                    string LastName = result.Rows[0][2].ToString();
                    string LanguageCode = result.Rows[0][3].ToString();

                    string token = SaveNewToken(UserID, db, Transaction);

                    // send the email with the link for resetting the password
                    string Domain = TAppSettingsManager.GetValue("Server.Url");
                    string EMailDomain = TAppSettingsManager.GetValue("Server.EmailDomain");
                    Dictionary<string, string> emailparameters = new Dictionary<string, string>();
                    emailparameters.Add("UserId", UserID);
                    emailparameters.Add("FirstName", FirstName);
                    emailparameters.Add("LastName", LastName);
                    emailparameters.Add("Domain", Domain);
                    emailparameters.Add("Token", token);
                    
                    new TSmtpSender().SendEmailFromTemplate(
                        "no-reply@" + EMailDomain,
                        "OpenPetra Admin",
                        AEmailAddress,
                        "requestnewpassword",
                        LanguageCode,
                        emailparameters);
                }

                return true;
            }
            catch (Exception e)
            {
                TLogging.Log("RequestNewPassword " + e.ToString());
                return false;
            }
            finally
            {
                db.CloseDBConnection();
            }
        }

        /// set a new password, authorized by the token that was sent via e-mail
        [NoRemoting]
        public static bool SetNewPassword(string AUserID, string AToken, string ANewPassword, out TVerificationResultCollection AVerificationResult)
        {
            TDataBase db = DBAccess.Connect("RequestNewPassword");
            TDBTransaction Transaction = db.BeginTransaction(IsolationLevel.Serializable, 0, "SetNewPassword1");
            AVerificationResult = new TVerificationResultCollection();

            try
            {
                // check if we have a valid token for this user
                string sql = "SELECT s_user_id_c" +
                    " FROM PUB_s_user WHERE UPPER(s_user_id_c) = ?" +
                    " AND s_password_reset_token_c = ?" + 
                    " AND s_password_reset_valid_until_d >= CURRENT_DATE()";

                OdbcParameter[] parameters = new OdbcParameter[2];
                parameters[0] = new OdbcParameter("UserID", OdbcType.VarChar);
                parameters[0].Value = AUserID.ToUpper();
                parameters[1] = new OdbcParameter("Token", OdbcType.VarChar);
                parameters[1].Value = AToken;

                DataTable result = db.SelectDT(sql, "user", Transaction, parameters);

                if (result.Rows.Count == 0)
                {
                    AVerificationResult.Add(new TVerificationResult("SetNewPassword",
                        "invalid token",
                        "invalid token",
                        "INVALID_TOKEN",
                        TResultSeverity.Resv_Critical));
                    TLogging.Log("SetNewPassword: invalid token for user " + AUserID);
                    return false;
                } else {
                    Transaction.Rollback();
                    UserInfo.SetUserInfo(new TPetraPrincipal(AUserID));
                    
                    if (SetUserPassword(AUserID,
                        ANewPassword,
                        false,
                        true,
                        String.Empty,
                        String.Empty,
                        out AVerificationResult))
                    {
                        Transaction = db.BeginTransaction(IsolationLevel.Serializable, 0, "SetNewPassword2");
                        string sqlUpdate = "UPDATE PUB_s_user SET s_password_reset_token_c = NULL, s_password_reset_valid_until_d = NULL" +
                            " WHERE s_user_id_c = ?";
                        parameters = new OdbcParameter[1];
                        parameters[0] = new OdbcParameter("UserID", OdbcType.VarChar);
                        parameters[0].Value = AUserID.ToUpper();
                        db.ExecuteNonQuery(sqlUpdate, Transaction, parameters, true);
                        return true;
                    }
                    else
                    {
                        TLogging.Log("SetNewPassword: cannot update password for user " + AUserID);
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                TLogging.Log("SetNewPassword " + e.ToString());
                return false;
            }
            finally
            {
                db.CloseDBConnection();
            }
        }

        /// <summary>
        /// sign up for self service
        /// </summary>
        [NoRemoting]
        public static bool SignUpSelfService(string AEmailAddress, string AFirstName, string ALastName, string APassword, string ALanguageCode)
        {
            TDataBase db = DBAccess.Connect("SignUpSelfService");
            TDBTransaction Transaction = db.BeginTransaction(IsolationLevel.Serializable, 0, "SignUpSelfService");
            string UserID;
            bool Result = true;

            try
            {
                // Create s_user entry. and permission PARTNERSELFSERVICE
                // s_user still not enabled, AccountLocked
                // don't create partner yet
                if (!CreateUser(AEmailAddress, APassword, AFirstName, ALastName, ALanguageCode, "PARTNERSELFSERVICE",
                    "PLACEHOLDERTOKEN", out UserID, db))
                {
                    Result = false;
                }
                else
                {
                    // set token on s_user.
                    string token = SaveNewToken(UserID, db, Transaction);

                    // send the email with the link for setting the password
                    string Domain = TAppSettingsManager.GetValue("Server.Url");
                    string EMailDomain = TAppSettingsManager.GetValue("Server.EmailDomain");
                    Dictionary<string, string> emailparameters = new Dictionary<string, string>();
                    emailparameters.Add("UserId", UserID);
                    emailparameters.Add("FirstName", AFirstName);
                    emailparameters.Add("LastName", ALastName);
                    emailparameters.Add("Domain", Domain);
                    emailparameters.Add("Token", token);
               
                    TSmtpSender sender = new TSmtpSender();
                    
                    if (!sender.SendEmailFromTemplate(
                        "no-reply@" + EMailDomain,
                        "OpenPetra Admin",
                        AEmailAddress,
                        "selfservicesignup",
                        ALanguageCode,
                        emailparameters))
                    {
                        throw new Exception("failure sending email. perhaps wrong EMailDomain? " + EMailDomain);
                    }
                }

            }
            catch (Exception e)
            {
                TLogging.Log("SetNewPassword " + e.ToString());
                return false;
            }
            finally
            {
                db.CloseDBConnection();
            }
            
            return Result;
        }

        /// create a new partner
        [NoRemoting]
        private static PPartnerRow CreateNewPartner(PartnerEditTDS AMainDS, TDataBase ADataBase)
        {
            PPartnerRow PartnerRow = AMainDS.PPartner.NewRowTyped();

            // get a new partner key
            Int64 newPartnerKey = -1;

            do
            {
                newPartnerKey = TNewPartnerKey.GetNewPartnerKey(DomainManager.GSiteKey, ADataBase);
                TNewPartnerKey.SubmitNewPartnerKey(DomainManager.GSiteKey, newPartnerKey, ref newPartnerKey, ADataBase);
                PartnerRow.PartnerKey = newPartnerKey;
            } while (newPartnerKey == -1);

            PartnerRow.StatusCode = MPartnerConstants.PARTNERSTATUS_ACTIVE;

            AMainDS.PPartner.Rows.Add(PartnerRow);

            TLogging.LogAtLevel(2, "Creating new partner: " + PartnerRow.PartnerKey.ToString());

            return PartnerRow;
        }

        private static PPartnerRow CreateNewFamilyPartner(PartnerEditTDS AMainDS, TDataBase ADataBase, string AFirstName, string AFamilyName)
        {
            PPartnerRow PartnerRow = CreateNewPartner(AMainDS, ADataBase);

            PartnerRow.PartnerClass = MPartnerConstants.PARTNERCLASS_FAMILY;
            PartnerRow.PartnerShortName = AFamilyName + "," + AFirstName;

            PFamilyRow FamilyRow = AMainDS.PFamily.NewRowTyped();
            FamilyRow.PartnerKey = PartnerRow.PartnerKey;
            FamilyRow.FamilyName = AFamilyName;
            FamilyRow.FirstName = AFirstName;
            AMainDS.PFamily.Rows.Add(FamilyRow);

            return PartnerRow;
        }

        /// create a new location
        private static void CreateNewLocation(Int64 APartnerKey, PartnerEditTDS AMainDS)
        {
            PLocationRow LocationRow = AMainDS.PLocation.NewRowTyped();

            LocationRow.SiteKey = DomainManager.GSiteKey;
            LocationRow.LocationKey = -1;
            LocationRow.CountryCode = "99";
            AMainDS.PLocation.Rows.Add(LocationRow);

            PPartnerLocationRow PartnerLocationRow = AMainDS.PPartnerLocation.NewRowTyped();
            PartnerLocationRow.SiteKey = LocationRow.SiteKey;
            PartnerLocationRow.PartnerKey = APartnerKey;
            PartnerLocationRow.LocationKey = LocationRow.LocationKey;
            AMainDS.PPartnerLocation.Rows.Add(PartnerLocationRow);
        }

        /// <summary>confirm the e-mail address for the self service account</summary>
        [NoRemoting]
        public static bool SignUpSelfServiceConfirm(string AUserID, string AToken)
        {
            TDataBase db = DBAccess.Connect("SignUpSelfServiceConfirm");
            TDBTransaction Transaction = new TDBTransaction();
            bool SubmissionOK = false;
            bool Result = false;

            db.WriteTransaction(ref Transaction, ref SubmissionOK,
                delegate
                {
                    // check if token is valid, and user still not activated
                    SUserTable UserDT = SUserAccess.LoadByPrimaryKey(AUserID, Transaction);

                    if (UserDT.Rows.Count != 1)
                    {
                        return;
                    }

                    SUserRow UserDR = UserDT[0];

                    if (UserDR.AccountLocked == false)
                    {
                        // the account has already been activated
                        Result = true;
                        return;
                    }

                    if (!((UserDR.PasswordResetToken == AToken) && (UserDR.PasswordResetValidUntil >= DateTime.Today)))
                    {
                        // invalid token
                        return;
                    }

                    try
                    {
                        // create partner record, and link to user record
                        PartnerEditTDS MainDS = new PartnerEditTDS();
                        PPartnerRow PartnerDR = CreateNewFamilyPartner(MainDS, db, UserDR.FirstName, UserDR.LastName);
                        CreateNewLocation(PartnerDR.PartnerKey, MainDS);
                        PartnerEditTDSAccess.SubmitChanges(MainDS, db);

                        // activate user record
                        UserDR.PartnerKey = PartnerDR.PartnerKey;
                        UserDR.PasswordResetToken = "";
                        UserDR.SetPasswordResetValidUntilNull();
                        UserDR.AccountLocked = false;
                        SUserAccess.SubmitChanges(UserDT, Transaction);

                        TUserAccountActivityLog.AddUserAccountActivityLogEntry(UserDR.UserId,
                            TUserAccountActivityLog.USER_CONFIRMED_SELF_SERVICE,
                            String.Format(Catalog.GetString(
                                    "The user {0} got confirmed."), UserDR.UserId),
                            Transaction);

                        SubmissionOK = true;
                        Result = true;
                    }
                    catch (Exception Exc)
                    {
                        TLogging.Log("An Exception occured during SignUpSelfServiceConfirm:" + Environment.NewLine +
                            Exc.ToString());

                        throw;
                    }
                });

            db.CloseDBConnection();

            return Result;
        }

        /// <summary>
        /// this is called from the MaintainUsers screen, for adding users, retiring users, set the password, etc
        /// </summary>
        [RequireModulePermission("SYSMAN")]
        public static TSubmitChangesResult SaveSUser(ref MaintainUsersTDS ASubmitDS,
            string AClientComputerName, string AClientIPAddress)
        {
            TSubmitChangesResult ReturnValue = TSubmitChangesResult.scrError;
            TDBTransaction SubmitChangesTransaction = new TDBTransaction();
            bool CanCreateUser;
            bool CanChangePassword;
            bool CanChangePermissions;
            int PwdSchemeVersionUpTillNow;
            int CurrentPwdSchemeVersion = TPasswordHelper.CurrentPasswordSchemeNumber;
            MaintainUsersTDS SubmitDS;

            SubmitDS = ASubmitDS;

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
            SubmitChangesTransaction = new TDBTransaction();
            bool SubmitOK = false;
            DBAccess.WriteTransaction(ref SubmitChangesTransaction,
                ref SubmitOK, delegate
                {
                    if (SubmitDS.SUser != null)
                    {
                        foreach (SUserRow user in SubmitDS.SUser.Rows)
                        {
                            // for new users: create users on the alternative authentication method
                            if (user.RowState == DataRowState.Added)
                            {
                                if (user.PwdSchemeVersion == 0)
                                {
                                    string UserID;
                                    // in fact, PasswordHash is expected to be the password in clear text
                                    CreateUser(user.UserId, user.PasswordHash, user.FirstName, user.LastName, string.Empty,
                                        string.Empty, string.Empty,
                                        out UserID,
                                        SubmitChangesTransaction.DataBaseObj);
                                    // this is a hack: the user is not stored here, but in CreateUser
                                    user.AcceptChanges();
                                }
                            }
                            else
                            {
                                PwdSchemeVersionUpTillNow = user.PwdSchemeVersion;

                                // Has the 'Account Locked' state changed?
                                if (Convert.ToBoolean(user[SUserTable.GetAccountLockedDBName(), DataRowVersion.Original]) != user.AccountLocked)
                                {
                                    if (user.AccountLocked)
                                    {
                                        TUserAccountActivityLog.AddUserAccountActivityLogEntry(user.UserId,
                                            TUserAccountActivityLog.USER_ACTIVITY_USER_ACCOUNT_GOT_LOCKED,
                                            String.Format(
                                                StrUserChangedOtherUsersLockedState, UserInfo.GetUserInfo().UserID,
                                                user.UserId, Catalog.GetString("locked")) +
                                            String.Format(ResourceTexts.StrRequestCallerInfo, AClientComputerName, AClientIPAddress),
                                            SubmitChangesTransaction);
                                    }
                                    else
                                    {
                                        TUserAccountActivityLog.AddUserAccountActivityLogEntry(user.UserId,
                                            TUserAccountActivityLog.USER_ACTIVITY_USER_ACCOUNT_GOT_UNLOCKED,
                                            String.Format(
                                                StrUserChangedOtherUsersLockedState, UserInfo.GetUserInfo().UserID,
                                                user.UserId, Catalog.GetString("unlocked")) +
                                            String.Format(ResourceTexts.StrRequestCallerInfo, AClientComputerName, AClientIPAddress),
                                            SubmitChangesTransaction);

                                        // If the user account got locked when a Password Hashing Scheme was in place that isn't
                                        // the current one then require the user to change his/her password on next login. This is to
                                        // ensure that the Password Hash and Salt that gets placed in the s_user table record of this
                                        // user at his/her next logon isn't just the new Password Hash and Salt of the password that
                                        // the user had used when the user account got Locked (the Password Hashing Scheme of that user
                                        // will get upgraded to the current one then, but in case the system administrator locked the user
                                        // account because (s)he suspects a security breach then any future attempts to use the previous
                                        // password will be thwarted).
                                        if (PwdSchemeVersionUpTillNow != CurrentPwdSchemeVersion)
                                        {
                                            user.PasswordNeedsChange = true;
                                        }
                                    }
                                }

                                // Has the 'Retired' state changed?
                                if (Convert.ToBoolean(user[SUserTable.GetRetiredDBName(), DataRowVersion.Original]) != user.Retired)
                                {
                                    if (user.Retired)
                                    {
                                        TUserAccountActivityLog.AddUserAccountActivityLogEntry(user.UserId,
                                            TUserAccountActivityLog.USER_ACTIVITY_USER_GOT_RETIRED,
                                            String.Format(
                                                StrUserChangedOtherUsersRetiredState, UserInfo.GetUserInfo().UserID,
                                                user.UserId, Catalog.GetString("retired")) +
                                            String.Format(ResourceTexts.StrRequestCallerInfo, AClientComputerName, AClientIPAddress),
                                            SubmitChangesTransaction);
                                    }
                                    else
                                    {
                                        TUserAccountActivityLog.AddUserAccountActivityLogEntry(user.UserId,
                                            TUserAccountActivityLog.USER_ACTIVITY_USER_GOT_UNRETIRED,
                                            String.Format(
                                                StrUserChangedOtherUsersRetiredState, UserInfo.GetUserInfo().UserID,
                                                user.UserId, Catalog.GetString("no longer retired")) +
                                            String.Format(ResourceTexts.StrRequestCallerInfo, AClientComputerName, AClientIPAddress),
                                            SubmitChangesTransaction);

                                        // If the user account got retired when a Password Hashing Scheme was in place that isn't
                                        // the current one then require the user to change his/her password on next login. This is to
                                        // ensure that the Password Hash and Salt that gets placed in the s_user table record of this
                                        // user at his/her next logon isn't just the new Password Hash and Salt of the password that
                                        // the user had used when the user account got Retired (the Password Hashing Scheme of that user
                                        // will get upgraded to the current one then, but in case the system administrator retired the user
                                        // account because (s)he suspects a security breach then any future attempts to use the previous
                                        // password will be thwarted).
                                        if (PwdSchemeVersionUpTillNow != CurrentPwdSchemeVersion)
                                        {
                                            user.PasswordNeedsChange = true;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    try
                    {
                        MaintainUsersTDSAccess.SubmitChanges(SubmitDS, SubmitChangesTransaction.DataBaseObj);

                        ReturnValue = TSubmitChangesResult.scrOK;
                        SubmitOK = true;
                    }
                    catch (Exception e)
                    {
                        TLogging.Log(e.Message);
                        TLogging.Log(e.StackTrace);

                        throw;
                    }
                });

            ASubmitDS = SubmitDS;

            return ReturnValue;
        }

        internal static void SetNewPasswordHashAndSaltForUser(SUserRow AUserDR, string ANewPassword,
            string AClientComputerName, string AClientIPAddress, TDBTransaction ATransaction)
        {
            byte[] Salt;
            int PwdSchemeVersionUpTillNow = AUserDR.PwdSchemeVersion;
            int CurrentPwdSchemeVersion = TPasswordHelper.CurrentPasswordSchemeNumber;

            EnsurePasswordHashingSchemeChangeIsAllowed(AUserDR.UserId, PwdSchemeVersionUpTillNow);

            // Note: In this Method we deliberately ignore the present value of the PwdSchemeVersion Column of AUserDR
            // because we *always* want to save the new password with the password hash of the current (=newest) version!

            // We always assign a new 'Salt' with every password change (best practice)!
            Salt = TPasswordHelper.CurrentPasswordScheme.GetNewPasswordSalt();

            AUserDR.PasswordSalt = Convert.ToBase64String(Salt);
            AUserDR.PasswordHash = TUserManagerWebConnector.CreateHashOfPassword(ANewPassword,
                Convert.ToBase64String(Salt), CurrentPwdSchemeVersion);

            if (PwdSchemeVersionUpTillNow != CurrentPwdSchemeVersion)
            {
                // Ensure AUserDR.PwdSchemeVersion gets set to the current (=newest) version!
                AUserDR.PwdSchemeVersion = CurrentPwdSchemeVersion;

                TUserAccountActivityLog.AddUserAccountActivityLogEntry(AUserDR.UserId,
                    TUserAccountActivityLog.USER_ACTIVITY_PWD_HASHING_SCHEME_UPGRADED,
                    String.Format(Catalog.GetString(
                            "The Password Scheme of User {0} got upgraded to {1}; previously it was {2}. "), AUserDR.UserId,
                        TPasswordHelper.CurrentPasswordSchemeNumber, PwdSchemeVersionUpTillNow) +
                    String.Format(ResourceTexts.StrRequestCallerInfo, AClientComputerName, AClientIPAddress),
                    ATransaction);
            }
        }

        internal static void SetNewPasswordHashAndSaltForNewUser(SUserRow AUserDR, string ANewPassword)
        {
            // We always assign a new 'Salt' with every password change (best practice)!
            byte[] Salt = TPasswordHelper.CurrentPasswordScheme.GetNewPasswordSalt();

            AUserDR.PwdSchemeVersion = TPasswordHelper.CurrentPasswordSchemeNumber;
            AUserDR.PasswordSalt = Convert.ToBase64String(Salt);
            AUserDR.PasswordHash = TUserManagerWebConnector.CreateHashOfPassword(ANewPassword,
                AUserDR.PasswordSalt, AUserDR.PwdSchemeVersion);
        }

        private static void EnsurePasswordHashingSchemeChangeIsAllowed(string AUserId, int APwdSchemeVersionUpTillNow)
        {
            string Message = String.Format("Unsupported downgrade of Password Hashing Scheme for User '{0}' encountered; aborting " +
                "(Password Hashing Scheme is {1} at present and an attempt was made to downgrade it to {2})",
                AUserId, APwdSchemeVersionUpTillNow, TPasswordHelper.CurrentPasswordSchemeNumber);

            if (TPasswordHelper.CurrentPasswordSchemeNumber < APwdSchemeVersionUpTillNow)
            {
                TLogging.Log(Message);

                throw new EPetraSecurityException(Message);
            }
        }
    }
}
