//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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
using System.Text;
using System.IO;
using System.Security.Principal;
using System.Security.Cryptography;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Exceptions;
using Ict.Common.Verification;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Common.Session;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Security;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MSysMan.Data.Access;
using Ict.Petra.Server.MSysMan.Maintenance.SystemDefaults.WebConnectors;
using Ict.Petra.Shared.Interfaces.Plugins.MSysMan;
using Ict.Petra.Server.MSysMan.Maintenance.WebConnectors;
using Ict.Petra.Server.MSysMan.Security.UserManager.WebConnectors;
using System.Threading;
using System.Globalization;

namespace Ict.Petra.Server.MSysMan.Security.UserManager.WebConnectors
{
    /// <summary>
    /// The TUserManager class provides access to the security-related information
    /// of Users of a Petra DB.
    /// </summary>
    /// <remarks>
    /// Calls methods that have the same name in the
    /// Ict.Petra.Server.App.Core.Security.UserManager Namespace to perform its
    /// functionality!
    ///
    /// This is required in two places,
    /// because it is needed before the appdomain is loaded and therefore cannot be in MSysMan;
    /// and it is needed here to make it available to the client via MSysMan remotely
    /// </remarks>
    public class TUserManagerWebConnector
    {
        private static IUserAuthentication FUserAuthenticationClass = null;

        /// <summary>
        /// load the plugin assembly for authentication
        /// </summary>
        [NoRemoting]
        public static IUserAuthentication LoadAuthAssembly(string AUserAuthenticationMethod)
        {
            if (FUserAuthenticationClass == null)
            {
                // namespace of the class TUserAuthentication, eg. Plugin.AuthenticationPhpBB
                // the dll has to be in the normal application directory
                string Namespace = AUserAuthenticationMethod;
                string NameOfDll = TAppSettingsManager.ApplicationDirectory + Path.DirectorySeparatorChar + Namespace + ".dll";
                string NameOfClass = Namespace + ".TUserAuthentication";

                // dynamic loading of dll
                System.Reflection.Assembly assemblyToUse = System.Reflection.Assembly.LoadFrom(NameOfDll);
                System.Type CustomClass = assemblyToUse.GetType(NameOfClass);

                FUserAuthenticationClass = (IUserAuthentication)Activator.CreateInstance(CustomClass);
            }

            return FUserAuthenticationClass;
        }

        /// <summary>
        /// load details of user
        /// </summary>
        [NoRemoting]
        internal static SUserRow LoadUser(String AUserID, out TPetraPrincipal APetraPrincipal)
        {
            SUserRow ReturnValue;

            TPetraIdentity PetraIdentity;

            ReturnValue = LoadUser(AUserID, out PetraIdentity);

            APetraPrincipal = new TPetraPrincipal(PetraIdentity, TGroupManager.LoadUserGroups(
                    AUserID), TTableAccessPermissionManager.LoadTableAccessPermissions(
                    AUserID), TModuleAccessManager.LoadUserModules(AUserID));

/*
 *          TLogging.LogAtLevel (8, "APetraPrincipal.IsTableAccessOK(tapMODIFY, 'p_person'): " +
 *                  APetraPrincipal.IsTableAccessOK(TTableAccessPermission.tapMODIFY, "p_person").ToString());
 */
            return ReturnValue;
        }

        /// <summary>
        /// Loads the details of the user from the s_user DB Table.
        /// </summary>
        /// <param name="AUserID">User ID to load the details for.</param>
        /// <param name="APetraIdentity">An instance of <see cref="TPetraIdentity"/> that is populated according
        /// to data in the s_user record.</param>
        /// <returns>s_user record of the User (if the user exists).</returns>
        /// <exception cref="EUserNotExistantException">Throws <see cref="EUserNotExistantException"/> if the user
        /// doesn't exist!</exception>
        [NoRemoting]
        private static SUserRow LoadUser(String AUserID, out TPetraIdentity APetraIdentity)
        {
            SUserRow ReturnValue;
            TDBTransaction ReadTransaction = null;
            SUserTable UserDT = null;
            SUserRow UserDR;
            DateTime LastLoginDateTime;
            DateTime FailedLoginDateTime;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(
                IsolationLevel.Serializable, TEnforceIsolationLevel.eilMinimum, ref ReadTransaction,
                delegate
                {
                    // Check if user exists in s_user DB Table
                    if (!SUserAccess.Exists(AUserID, ReadTransaction))
                    {
                        throw new EUserNotExistantException(StrInvalidUserIDPassword);
                    }

                    // User exists, so load User record
                    UserDT = SUserAccess.LoadByPrimaryKey(AUserID, ReadTransaction);
                });

            UserDR = UserDT[0];

            if (!UserDR.IsFailedLoginDateNull())
            {
                FailedLoginDateTime = UserDR.FailedLoginDate.Value;
                FailedLoginDateTime = FailedLoginDateTime.AddSeconds(Convert.ToDouble(UserDR.FailedLoginTime));
            }
            else
            {
                FailedLoginDateTime = DateTime.MinValue;
            }

            if (!UserDR.IsLastLoginDateNull())
            {
                LastLoginDateTime = UserDR.LastLoginDate.Value;
                LastLoginDateTime = LastLoginDateTime.AddSeconds(Convert.ToDouble(UserDR.LastLoginTime));
            }
            else
            {
                LastLoginDateTime = DateTime.MinValue;
            }

            Int64 PartnerKey;

            if (!UserDR.IsPartnerKeyNull())
            {
                PartnerKey = UserDR.PartnerKey;
            }
            else
            {
                // to make it not match PartnerKey 0, which might be stored in the DB or in a variable
                PartnerKey = -1;
            }

            // Create PetraIdentity
            APetraIdentity = new Ict.Petra.Shared.Security.TPetraIdentity(
                AUserID.ToUpper(), UserDR.LastName, UserDR.FirstName, UserDR.LanguageCode, UserDR.AcquisitionCode, DateTime.MinValue,
                LastLoginDateTime, FailedLoginDateTime, UserDR.FailedLogins, PartnerKey, UserDR.DefaultLedgerNumber, UserDR.AccountLocked,
                UserDR.Retired, UserDR.CanModify);
            ReturnValue = UserDR;

            return ReturnValue;
        }

        /// <summary>
        /// Authenticate a user.
        /// </summary>
        /// <param name="AUserID">User ID.</param>
        /// <param name="APassword">Password.</param>
        /// <param name="AClientComputerName">Name of the Client Computer that the authentication request came from.</param>
        /// <param name="AClientIPAddress">IP Address of the Client Computer that the authentication request came from.</param>
        /// <param name="ASystemEnabled">True if the system is enabled, otherwise false.</param>
        /// <returns>An instance of <see cref="TPetraPrincipal"/> if the authentication was successful, otherwise null.</returns>
        [NoRemoting]
        public static TPetraPrincipal PerformUserAuthentication(String AUserID, String APassword,
            string AClientComputerName, string AClientIPAddress, out Boolean ASystemEnabled)
        {
            DateTime LoginDateTime;
            TPetraPrincipal PetraPrincipal = null;
            string UserAuthenticationMethod = TAppSettingsManager.GetValue("UserAuthenticationMethod", "OpenPetraDBSUser", false);
            IUserAuthentication AuthenticationAssembly;
            string AuthAssemblyErrorMessage;

            Int32 AProcessID = -1;

            ASystemEnabled = true;

            string EmailAddress = AUserID;

            if (AUserID.Contains("@"))
            {
                AUserID = AUserID.Substring(0, AUserID.IndexOf("@")).
                          Replace(".", string.Empty).
                          Replace("_", string.Empty).ToUpper();
            }

            SUserRow UserDR = LoadUser(AUserID, out PetraPrincipal);

            try
            {
                UserInfo.GUserInfo = PetraPrincipal;

                if ((AUserID == "SYSADMIN") && TSession.HasVariable("ServerAdminToken"))
                {
                    // Login via server admin console authenticated by file token
                }
                //
                // (1) Check user-supplied password
                //
                else if (UserAuthenticationMethod == "OpenPetraDBSUser")
                {
                    // TODO see PwdSchemeVersion in commit 3285
                    if (UserDR.PasswordSalt.Length != 32) // old length was 44
                    {
                        // password has not been updated yet to new hash
                        if (CreateHashOfPassword(String.Concat(APassword,
                                    UserDR.PasswordSalt)) != UserDR.PasswordHash)
                        {
                            // The password that the user supplied is wrong!!! --> Save failed user login attempt!
                            // If the number of permitted failed logins in a row gets exceeded then also lock the user account!
                            SaveFailedLogin(AUserID, UserDR, AClientComputerName, AClientIPAddress);

                            if (UserDR.AccountLocked)
                            {
                                // User Account just got locked!
                                throw new EUserAccountGotLockedException(StrInvalidUserIDPassword);
                            }
                            else
                            {
                                throw new EPasswordWrongException(StrInvalidUserIDPassword);
                            }
                        }
                        else
                        {
                            // TODO update password with new hash
                            // see SetNewPasswordHashAndSaltForUser in commit 3285
                        }
                    }
                    else if (!PasswordHelper.EqualsAntiTimingAttack(
                                Convert.FromBase64String(CreateHashOfPassword(APassword, UserDR.PasswordSalt)),
                                Convert.FromBase64String(UserDR.PasswordHash)))
                    {
                        // The password that the user supplied is wrong!!! --> Save failed user login attempt!
                        // If the number of permitted failed logins in a row gets exceeded then also lock the user account!
                        SaveFailedLogin(AUserID, UserDR, AClientComputerName, AClientIPAddress);

                        if (UserDR.AccountLocked)
                        {
                            // User Account just got locked!
                            throw new EUserAccountGotLockedException(StrInvalidUserIDPassword);
                        }
                        else
                        {
                            throw new EPasswordWrongException(StrInvalidUserIDPassword);
                        }
                    }
                }
                else
                {
                    AuthenticationAssembly = LoadAuthAssembly(UserAuthenticationMethod);

                    if (!AuthenticationAssembly.AuthenticateUser(EmailAddress, APassword, out AuthAssemblyErrorMessage))
                    {
                        // The password that the user supplied is wrong!!! --> Save failed user login attempt!
                        // If the number of permitted failed logins in a row gets exceeded then also lock the user account!
                        SaveFailedLogin(AUserID, UserDR, AClientComputerName, AClientIPAddress);

                        if (UserDR.AccountLocked)
                        {
                            // User Account just got locked!
                            throw new EUserAccountGotLockedException(StrInvalidUserIDPassword);
                        }
                        else
                        {
                            throw new EPasswordWrongException(AuthAssemblyErrorMessage);
                        }
                    }
                }

                //
                // (2) Check if the User Account is Locked or if the user is 'Retired'. If either is true then deny the login!!!
                //
                // IMPORTANT: We perform these checks only AFTER the check for the correctness of the password so that every
                // log-in attempt that gets rejected on grounds of a wrong password takes the same amount of time (to help prevent
                // an attack vector called 'timing attack')
                if (PetraPrincipal.PetraIdentity.AccountLocked || PetraPrincipal.PetraIdentity.Retired)
                {
                    if (PetraPrincipal.PetraIdentity.AccountLocked)
                    {
                        // Logging
                        TLoginLog.AddLoginLogEntry(AUserID, false, "User attempted to log in, but the user account was locked!  " +
                            String.Format(ResourceTexts.StrRequestCallerInfo, AClientComputerName, AClientIPAddress),
                            out AProcessID);

                        // Only now throw the Exception!
                        throw new EUserAccountLockedException(StrInvalidUserIDPassword);
                    }
                    else
                    {
                        // Logging
                        TLoginLog.AddLoginLogEntry(AUserID, false, "User attempted to log in, but the user is retired!  " +
                            String.Format(ResourceTexts.StrRequestCallerInfo, AClientComputerName, AClientIPAddress),
                            out AProcessID);

                        // Only now throw the Exception!
                        throw new EUserRetiredException(StrInvalidUserIDPassword);
                    }
                }

                //
                // (3) Check SystemLoginStatus (whether the general use of the OpenPetra application is enabled/disabled) in the
                // SystemStatus table (this table always holds only a single record)
                //
                Boolean NewTransaction = false;
                SSystemStatusTable SystemStatusDT;

                TDBTransaction ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);

                try
                {
                    SystemStatusDT = SSystemStatusAccess.LoadAll(ReadTransaction);
                }
                finally
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
                        TLogging.LogAtLevel(7, "TUserManager.PerformUserAuthentication: committed own transaction.");
                    }
                }

                if (SystemStatusDT[0].SystemLoginStatus)
                {
                    ASystemEnabled = true;
                }
                else
                {
                    ASystemEnabled = false;

                    // TODO: Check for Security Group membership might need reviewal when security model of OpenPetra might get reviewed...
                    if (PetraPrincipal.IsInGroup("SYSADMIN"))
                    {
                        PetraPrincipal.LoginMessage =
                            String.Format(StrSystemDisabled1,
                                SystemStatusDT[0].SystemDisabledReason) + Environment.NewLine + Environment.NewLine +
                            StrSystemDisabled2Admin;
                    }
                    else
                    {
                        TLoginLog.AddLoginLogEntry(AUserID, false, "User wanted to log in, but the System was disabled.  " +
                            String.Format(ResourceTexts.StrRequestCallerInfo, AClientComputerName, AClientIPAddress),
                            true, out AProcessID);

                        throw new ESystemDisabledException(String.Format(StrSystemDisabled1,
                                SystemStatusDT[0].SystemDisabledReason) + Environment.NewLine + Environment.NewLine +
                            String.Format(StrSystemDisabled2, StringHelper.DateToLocalizedString(SystemStatusDT[0].SystemAvailableDate.Value),
                                SystemStatusDT[0].SystemAvailableDate.Value.AddSeconds(SystemStatusDT[0].SystemAvailableTime).ToShortTimeString()));
                    }
                }

                //
                // (4) Save successful login!
                //
                LoginDateTime = DateTime.Now;
                UserDR.LastLoginDate = LoginDateTime;
                UserDR.LastLoginTime = Conversions.DateTimeToInt32Time(LoginDateTime);
                UserDR.FailedLogins = 0;  // this needs resetting!

                SaveUser(AUserID, (SUserTable)UserDR.Table);

                PetraPrincipal.PetraIdentity.CurrentLogin = LoginDateTime;

                //PetraPrincipal.PetraIdentity.FailedLogins = 0;

                // TODO: Check for Security Group membership might need reviewal when security model of OpenPetra might get reviewed...

                if (PetraPrincipal.IsInGroup("SYSADMIN"))
                {
                    TLoginLog.AddLoginLogEntry(AUserID, true, "User login - SYSADMIN privileges.  " +
                        String.Format(ResourceTexts.StrRequestCallerInfo, AClientComputerName, AClientIPAddress), out AProcessID);
                }
                else
                {
                    TLoginLog.AddLoginLogEntry(AUserID, true, "User login.  " +
                        String.Format(ResourceTexts.StrRequestCallerInfo, AClientComputerName, AClientIPAddress), out AProcessID);
                }

                PetraPrincipal.ProcessID = AProcessID;
                AProcessID = 0;

                //
                // (5) Check if a password change is requested for this user
                //
                if (UserDR.PasswordNeedsChange)
                {
                    // The user needs to change their password before they can use OpenPetra
                    PetraPrincipal.LoginMessage = SharedConstants.LOGINMUSTCHANGEPASSWORD;
                }
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return PetraPrincipal;
        }

        /// <summary>
        /// Save a failed user login attempt. If the number of permitted failed logins in a row gets exceeded then the
        /// user account gets Locked, too!
        /// </summary>
        /// <param name="AUserID">User ID.</param>
        /// <param name="UserDR">s_user DataRow of the user.</param>
        /// <param name="AClientComputerName">Name of the Client Computer that the authentication request came from.</param>
        /// <param name="AClientIPAddress">IP Address of the Client Computer that the authentication request came from.</param>
        private static void SaveFailedLogin(string AUserID, SUserRow UserDR,
            string AClientComputerName, string AClientIPAddress)
        {
            int AProcessID;
            int FailedLoginsUntilAccountGetsLocked =
                TSystemDefaults.GetInt32Default(SharedConstants.SYSDEFAULT_FAILEDLOGINS_UNTIL_ACCOUNT_GETS_LOCKED, 10);

            // Console.WriteLine('PetraPrincipal.PetraIdentity.FailedLogins: ' + PetraPrincipal.PetraIdentity.FailedLogins.ToString +
            // '; PetraPrincipal.PetraIdentity.AccountLocked: ' + PetraPrincipal.PetraIdentity.AccountLocked.ToString);

            UserDR.FailedLogins++;
            UserDR.FailedLoginDate = DateTime.Now;
            UserDR.FailedLoginTime = Conversions.DateTimeToInt32Time(UserDR.FailedLoginDate.Value);

            // Check if User Account should be Locked due to too many successive failed log-in attempts
            if ((UserInfo.GUserInfo.PetraIdentity.FailedLogins >= FailedLoginsUntilAccountGetsLocked)
                && ((!UserInfo.GUserInfo.PetraIdentity.AccountLocked)))
            {
                // Lock User Account (this user will no longer be able to log in until a Sysadmin resets this flag!)
                UserDR.AccountLocked = true;
            }

            // Logging
            TLoginLog.AddLoginLogEntry(AUserID, false, String.Format("User supplied wrong password!  (Failed Logins: now {0}; " +
                    "Account Locked: now {1}, User Retired: {2})  ", UserDR.FailedLogins, UserDR.AccountLocked, UserDR.Retired) +
                String.Format(ResourceTexts.StrRequestCallerInfo, AClientComputerName, AClientIPAddress), out AProcessID);

            SaveUser(AUserID, (SUserTable)UserDR.Table);
        }

        /// <summary>
        /// Call this Method when a log-in is attempted for a non-existing user (!) so that the time that is spent on
        /// 'authenticating' them is as long as is spent on authenticating existing users. This is done so that an attacker
        /// that tries to perform user authentication with 'username guessing' cannot easily tell that the user doesn't exist by
        /// checking the time in which the server returns an error (this is an attack vector called 'timing attack')!
        /// </summary>
        public static void SimulatePasswordAuthenticationForNonExistingUser()
        {
            string UserAuthenticationMethod = TAppSettingsManager.GetValue("UserAuthenticationMethod", "OpenPetraDBSUser", false);

            if (UserAuthenticationMethod == "OpenPetraDBSUser")
            {
                TUserManagerWebConnector.CreateHashOfPassword("wrongPassword", PasswordHelper.GetNewPasswordSalt());
            }
            else
            {
                IUserAuthentication auth = TUserManagerWebConnector.LoadAuthAssembly(UserAuthenticationMethod);

                string ErrorMessage;

                auth.AuthenticateUser("wrongUser", "wrongPassword", out ErrorMessage);
            }
        }

        #region Resourcestrings

        private static readonly string StrSystemDisabled1 = Catalog.GetString("OpenPetra is currently disabled due to {0}.");

        private static readonly string StrSystemDisabled2 = Catalog.GetString("It will be available on {0} at {1}.");

        private static readonly string StrSystemDisabled2Admin = Catalog.GetString("Proceed with caution.");

        private static readonly string StrInvalidUserIDPassword = Catalog.GetString("Invalid User ID or Password.");

        #endregion

        /// <summary>
        /// create hash of password and the salt.
        /// replacement for FormsAuthentication.HashPasswordForStoringInConfigFile
        /// which is part of System.Web.dll and not available in the client profile of .net v4.0
        /// </summary>
        /// <returns></returns>
        [NoRemoting]
        public static string CreateHashOfPassword(string APassword, string ASalt, string AHashType = "Scrypt")
        {
            if (AHashType.ToUpper() == "MD5")
            {
                return BitConverter.ToString(
                    MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(String.Concat(APassword,
                                ASalt)))).Replace("-", "");
            }
            else if (AHashType.ToUpper() == "SCRYPT")
            {
                return PasswordHelper.GetPasswordHash(APassword, ASalt);
            }
            else
            {
                throw new ArgumentException("Unsupported AHashType argument value '" + AHashType + "'; supported types are 'MD5' and 'Scrypt'");
            }
        }

        /// <summary>
        /// Causes an immediately reload of the UserInfo that is stored in a global
        /// variable.
        /// </summary>
        [RequireModulePermission("NONE")]
        public static TPetraPrincipal ReloadCachedUserInfo()
        {
            try
            {
                TPetraPrincipal UserDetails;
                LoadUser(UserInfo.GUserInfo.UserID, out UserDetails);
                UserInfo.GUserInfo = UserDetails;
            }
            catch (Exception Exp)
            {
                TLogging.Log("Exception occured in ReloadCachedUserInfo: " + Exp.ToString());
                throw;
            }

            return UserInfo.GUserInfo;
        }

        /// <summary>
        /// save user details (last login time, failed logins etc)
        /// </summary>
        [NoRemoting]
        private static Boolean SaveUser(String AUserID, SUserTable AUserDataTable)
        {
            TDBTransaction TheTransaction;

            if ((AUserDataTable != null) && (AUserDataTable.Rows.Count > 0))
            {
                TheTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

                try
                {
                    SUserAccess.SubmitChanges(AUserDataTable, TheTransaction);

                    DBAccess.GDBAccessObj.CommitTransaction();
                }
                catch (Exception Exc)
                {
                    TLogging.Log("An Exception occured during the saving of a User:" + Environment.NewLine + Exc.ToString());

                    DBAccess.GDBAccessObj.RollbackTransaction();

                    throw;
                }
            }
            else
            {
                // nothing to save!
                return false;
            }

            return true;
        }

        /// <summary>
        /// Queues a ClientTask for reloading of the UserInfo for all connected Clients
        /// with a certain UserID.
        ///
        /// </summary>
        /// <param name="AUserID">UserID for which the ClientTask should be queued
        /// </param>
        [RequireModulePermission("NONE")]
        public static void SignalReloadCachedUserInfo(String AUserID)
        {
            TClientManager.QueueClientTask(AUserID,
                SharedConstants.CLIENTTASKGROUP_USERINFOREFRESH,
                "",
                null, null, null, null,
                1,
                -1);
        }
    }
}

namespace Ict.Petra.Server.MSysMan.Maintenance.UserManagement
{
    /// <summary>
    /// this manager is called from Server.App.Core
    /// </summary>
    public class TUserManager : IUserManager
    {
        /// <summary>
        /// Adds a new user.
        /// </summary>
        public bool AddUser(string AUserID, string APassword = "")
        {
            return TMaintenanceWebConnector.CreateUser(AUserID,
                APassword,
                string.Empty,
                string.Empty,
                TMaintenanceWebConnector.DEMOMODULEPERMISSIONS);
        }

        /// <summary>
        /// Authenticate a user.
        /// </summary>
        /// <param name="AUserID">User ID.</param>
        /// <param name="APassword">Password.</param>
        /// <param name="AClientComputerName">Name of the Client Computer that the authentication request came from.</param>
        /// <param name="AClientIPAddress">IP Address of the Client Computer that the authentication request came from.</param>
        /// <param name="ASystemEnabled">True if the system is enabled, otherwise false.</param>
        /// <returns>An instance of <see cref="TPetraPrincipal"/> if the authentication was successful, otherwise null.</returns>
        public IPrincipal PerformUserAuthentication(string AUserID, string APassword,
            string AClientComputerName, string AClientIPAddress,
            out Boolean ASystemEnabled)
        {
            return TUserManagerWebConnector.PerformUserAuthentication(AUserID, APassword, AClientComputerName, AClientIPAddress,
                out ASystemEnabled);
        }

        /// <summary>
        /// Call this Method when a log-in is attempted for a non-existing user (!) so that the time that is spent on
        /// 'authenticating' them is as long as is spent on authenticating existing users. This is done so that an attacker
        /// that tries to perform user authentication with 'username guessing' cannot easily tell that the user doesn't exist by
        /// checking the time in which the server returns an error (this is an attack vector called 'timing attack')!
        /// </summary>
        public void SimulatePasswordAuthenticationForNonExistingUser()
        {
            TUserManagerWebConnector.SimulatePasswordAuthenticationForNonExistingUser();
         }
    }
}
