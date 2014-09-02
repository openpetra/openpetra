//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2013 by OM International
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
using System.Security.Cryptography;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.Plugins.MSysMan;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MSysMan.Validation;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MSysMan.Data.Access;
using Ict.Petra.Shared.Security;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MSysMan.Security.UserManager.WebConnectors;

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
        public static string DEMOMODULEPERMISSIONS = "DEMOMODULEPERMISSIONS";

        /// <summary>
        /// set the password of an existing user. this takes into consideration how users are authenticated in this system, by
        /// using an optional authentication plugin dll
        /// </summary>
        [RequireModulePermission("SYSMAN")]
        public static bool SetUserPassword(string AUsername, string APassword, bool APasswordNeedsChanged)
        {
            string UserAuthenticationMethod = TAppSettingsManager.GetValue("UserAuthenticationMethod", "OpenPetraDBSUser", false);

            if (UserAuthenticationMethod == "OpenPetraDBSUser")
            {
                TPetraPrincipal tempPrincipal;
                SUserRow UserDR = TUserManagerWebConnector.LoadUser(AUsername.ToUpper(), out tempPrincipal);
                SUserTable UserTable = (SUserTable)UserDR.Table;

                Random r = new Random();
                UserDR.PasswordSalt = r.Next(1000000000).ToString();
                UserDR.PasswordHash = TUserManagerWebConnector.CreateHashOfPassword(String.Concat(APassword,
                        UserDR.PasswordSalt), "SHA1");
                UserDR.PasswordNeedsChange = APasswordNeedsChanged;

                TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
                SUserAccess.SubmitChanges(UserTable, Transaction);

                DBAccess.GDBAccessObj.CommitTransaction();

                return true;
            }
            else
            {
                IUserAuthentication auth = TUserManagerWebConnector.LoadAuthAssembly(UserAuthenticationMethod);

                return auth.SetPassword(AUsername, APassword);
            }
        }

        /// <summary>
        /// set the password of the current user. this takes into consideration how users are authenticated in this system, by
        /// using an optional authentication plugin dll.
        /// any user can call this, but they need to know the old password.
        /// </summary>
        [RequireModulePermission("NONE")]
        public static bool SetUserPassword(string AUsername,
            string APassword,
            string AOldPassword,
            bool APasswordNeedsChanged,
            out TVerificationResultCollection AVerification)
        {
            TDBTransaction Transaction;
            string UserAuthenticationMethod = TAppSettingsManager.GetValue("UserAuthenticationMethod", "OpenPetraDBSUser", false);
            TVerificationResult VerificationResult;

            AVerification = new TVerificationResultCollection();

            if (!TSharedSysManValidation.CheckPasswordQuality(APassword, out VerificationResult))
            {
                return false;
            }

            if (UserAuthenticationMethod == "OpenPetraDBSUser")
            {
                TPetraPrincipal tempPrincipal;
                SUserRow UserDR = TUserManagerWebConnector.LoadUser(AUsername.ToUpper(), out tempPrincipal);

                if (TUserManagerWebConnector.CreateHashOfPassword(String.Concat(AOldPassword,
                            UserDR.PasswordSalt)) != UserDR.PasswordHash)
                {
                    AVerification = new TVerificationResultCollection();
                    AVerification.Add(new TVerificationResult("\nPassword quality check.",
                            String.Format(
                                Catalog.GetString(
                                    "Old password entered incorrectly. Password not changed.")),
                            TResultSeverity.Resv_Critical));
                    return false;
                }

                SUserTable UserTable = (SUserTable)UserDR.Table;

                UserDR.PasswordHash = TUserManagerWebConnector.CreateHashOfPassword(String.Concat(APassword,
                        UserDR.PasswordSalt));
                UserDR.PasswordNeedsChange = APasswordNeedsChanged;

                Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

                try
                {
                    SUserAccess.SubmitChanges(UserTable, Transaction);

                    DBAccess.GDBAccessObj.CommitTransaction();
                }
                catch (Exception Exc)
                {
                    TLogging.Log("An Exception occured during the setting of the User Password:" + Environment.NewLine + Exc.ToString());

                    DBAccess.GDBAccessObj.RollbackTransaction();

                    throw;
                }

                return true;
            }
            else
            {
                IUserAuthentication auth = TUserManagerWebConnector.LoadAuthAssembly(UserAuthenticationMethod);

                return auth.SetPassword(AUsername, APassword, AOldPassword);
            }
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
            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(
                IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum, ref ReadTransaction,
            delegate
            {            
                if (SUserAccess.Exists(newUser.UserId, null))
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
                    newUser.PasswordSalt = PasswordHelper.GetNewPasswordSalt();
                    newUser.PasswordHash = PasswordHelper.GetPasswordHash(APassword, newUser.PasswordSalt);
                    newUser.PasswordNeedsChange = true;
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
                        string[] tables = new string[] {
                            "p_bank", "p_church", "p_family", "p_location",
                            "p_organisation", "p_partner", "p_partner_location",
                            "p_partner_type", "p_person", "p_unit", "p_venue"
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
            MaintainUsersTDS ReturnValue = null;

            Boolean NewTransaction;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            try
            {
                ReturnValue = new MaintainUsersTDS();
                SUserAccess.LoadAll(ReturnValue, Transaction);
                SUserModuleAccessPermissionAccess.LoadAll(ReturnValue, Transaction);
                SModuleAccess.LoadAll(ReturnValue, Transaction);
            }
            catch (Exception e)
            {
                TLogging.Log(e.Message);
                TLogging.Log(e.StackTrace);
                ReturnValue = null;
            }

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

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
                    // If a password has been added for the first time there will be a (unecrypted) password and no salt.
                    // Create salt and hash.
                    else if ((user.PasswordHash.Length > 0) && user.IsPasswordSaltNull())
                    {
                        user.PasswordSalt = PasswordHelper.GetNewPasswordSalt();
                        user.PasswordHash = PasswordHelper.GetPasswordHash(user.PasswordHash, user.PasswordSalt);
                        user.PasswordNeedsChange = true;
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
                ReturnValue = TSubmitChangesResult.scrError;
            }

            return ReturnValue;
        }
    }
}