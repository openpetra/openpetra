//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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
using System.Text;
using System.IO;
using System.Security.Principal;
using System.Security.Cryptography;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Exceptions;
using Ict.Common.Verification;
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Security;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MSysMan.Data.Access;
using Ict.Petra.Shared.Interfaces.Plugins.MSysMan;
using Ict.Petra.Server.MSysMan.Maintenance.WebConnectors;
using Ict.Petra.Server.MSysMan.Security.UserManager.WebConnectors;

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
        public static SUserRow LoadUser(String AUserID, out TPetraPrincipal APetraPrincipal)
        {
            SUserRow ReturnValue;

            Ict.Petra.Shared.Security.TPetraIdentity PetraIdentity;
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
        /// load the details of the user
        /// </summary>
        /// <param name="AUserID"></param>
        /// <param name="APetraIdentity"></param>
        /// <returns></returns>
        [NoRemoting]
        public static SUserRow LoadUser(String AUserID, out TPetraIdentity APetraIdentity)
        {
            SUserRow ReturnValue;
            TDBTransaction ReadWriteTransaction;
            Boolean NewTransaction;
            SUserTable UserDT;
            SUserRow UserDR;
            Boolean UserExists;
            DateTime LastLoginDateTime;
            DateTime FailedLoginDateTime;

            ReadWriteTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            // Check if user exists in s_user DB Table
            try
            {
                UserExists = SUserAccess.Exists(AUserID, ReadWriteTransaction);
            }
            catch
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TUserManager.LoadUser: committed own transaction.");
                }

                throw;
            }

            if (!UserExists)
            {
                throw new EUserNotExistantException(StrInvalidUserIDPassword);
            }
            else
            {
                try
                {
                    // Load User record
                    UserDT = SUserAccess.LoadByPrimaryKey(AUserID, ReadWriteTransaction);
                }
                catch (Exception Exp)
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
                        TLogging.LogAtLevel(7, "TUserManager.LoadUser: committed own transaction.");
                    }

                    TLogging.LogAtLevel(8, "Exception occured while loading a s_user record: " + Exp.ToString());

                    throw;
                }

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TUserManager.LoadUser: committed own transaction.");
                }

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
                    LastLoginDateTime, FailedLoginDateTime, UserDR.FailedLogins, PartnerKey, UserDR.DefaultLedgerNumber, UserDR.Retired,
                    UserDR.CanModify);
                ReturnValue = UserDR;
            }

            return ReturnValue;
        }

        /// <summary>
        /// make sure the user can login with the correct password
        /// </summary>
        /// <param name="AUserID"></param>
        /// <param name="APassword"></param>
        /// <param name="AProcessID"></param>
        /// <param name="ASystemEnabled"></param>
        /// <returns></returns>
        [NoRemoting]
        public static TPetraPrincipal PerformUserAuthentication(String AUserID, String APassword, out Int32 AProcessID, out Boolean ASystemEnabled)
        {
            DateTime LoginDateTime;
            TPetraPrincipal PetraPrincipal = null;

            AProcessID = -1;
            ASystemEnabled = true;

            string EmailAddress = AUserID;

            if (AUserID.Contains("@"))
            {
                AUserID = AUserID.Substring(0, AUserID.IndexOf("@")).
                          Replace(".", string.Empty).
                          Replace("_", string.Empty).ToUpper();
            }

            try
            {
                SUserRow UserDR = LoadUser(AUserID, out PetraPrincipal);

                // Already assign the global variable here, because it is needed for SUserAccess.SubmitChanges later in this function
                UserInfo.GUserInfo = PetraPrincipal;

                // Check if user is retired
                if (PetraPrincipal.PetraIdentity.Retired)
                {
                    throw new EUserRetiredException(StrUserIsRetired);
                }

                // Console.WriteLine('PetraPrincipal.PetraIdentity.FailedLogins: ' + PetraPrincipal.PetraIdentity.FailedLogins.ToString +
                // '; PetraPrincipal.PetraIdentity.Retired: ' + PetraPrincipal.PetraIdentity.Retired.ToString);
                // Check if user should be autoretired
                if ((PetraPrincipal.PetraIdentity.FailedLogins >= 5) && ((!PetraPrincipal.PetraIdentity.Retired)))
                {
                    UserDR.Retired = true;
                    UserDR.FailedLogins = 4;

                    SaveUser(AUserID, (SUserTable)UserDR.Table);
                    
                    throw new EAccessDeniedException(StrUserIsRetired);
                }

                // Check SystemLoginStatus (Petra enabled/disabled) in the SystemStatus table (always holds only one record)
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

                    if (PetraPrincipal.IsInGroup("SYSADMIN"))
                    {
                        PetraPrincipal.LoginMessage =
                            String.Format(StrSystemDisabled1,
                                SystemStatusDT[0].SystemDisabledReason) + Environment.NewLine + Environment.NewLine + StrSystemDisabled2Admin;
                    }
                    else
                    {
                        TLoginLog.AddLoginLogEntry(AUserID, "System disabled", true, out AProcessID);

                        throw new ESystemDisabledException(String.Format(StrSystemDisabled1,
                                SystemStatusDT[0].SystemDisabledReason) + Environment.NewLine + Environment.NewLine +
                            String.Format(StrSystemDisabled2, StringHelper.DateToLocalizedString(SystemStatusDT[0].SystemAvailableDate.Value),
                                SystemStatusDT[0].SystemAvailableDate.Value.AddSeconds(SystemStatusDT[0].SystemAvailableTime).ToShortTimeString()));
                    }
                }

                string UserAuthenticationMethod = TAppSettingsManager.GetValue("UserAuthenticationMethod", "OpenPetraDBSUser", false);

                if (UserAuthenticationMethod == "OpenPetraDBSUser")
                {
                    // TODO 1 oChristianK cSecurity : Perform user authentication by verifying password hash in the DB
                    // see also ICTPetraWiki: Todo_Petra.NET#Implement_Security_.7B2.7D_.5BChristian.5D
                    if (CreateHashOfPassword(String.Concat(APassword,
                                UserDR.PasswordSalt)) != UserDR.PasswordHash)
                    {
                        // increase failed logins
                        UserDR.FailedLogins++;
                        LoginDateTime = DateTime.Now;
                        UserDR.FailedLoginDate = LoginDateTime;
                        UserDR.FailedLoginTime = Conversions.DateTimeToInt32Time(LoginDateTime);
                        SaveUser(AUserID, (SUserTable)UserDR.Table);

                        throw new EPasswordWrongException(Catalog.GetString("Invalid User ID/Password."));
                    }
                }
                else
                {
                    IUserAuthentication auth = LoadAuthAssembly(UserAuthenticationMethod);

                    string ErrorMessage;

                    if (!auth.AuthenticateUser(EmailAddress, APassword, out ErrorMessage))
                    {
                        UserDR.FailedLogins++;
                        LoginDateTime = DateTime.Now;
                        UserDR.FailedLoginDate = LoginDateTime;
                        UserDR.FailedLoginTime = Conversions.DateTimeToInt32Time(LoginDateTime);
                        SaveUser(AUserID, (SUserTable)UserDR.Table);

                        throw new EPasswordWrongException(ErrorMessage);
                    }
                }

                // Save successful login
                LoginDateTime = DateTime.Now;
                UserDR.LastLoginDate = LoginDateTime;
                UserDR.LastLoginTime = Conversions.DateTimeToInt32Time(LoginDateTime);
                UserDR.FailedLogins = 0;

                SaveUser(AUserID, (SUserTable)UserDR.Table);

                PetraPrincipal.PetraIdentity.CurrentLogin = LoginDateTime;

                // PetraPrincipal.PetraIdentity.FailedLogins := 0;

                if (PetraPrincipal.IsInGroup("SYSADMIN"))
                {
                    TLoginLog.AddLoginLogEntry(AUserID, "Successful  SYSADMIN", out AProcessID);
                }
                else
                {
                    TLoginLog.AddLoginLogEntry(AUserID, "Successful", out AProcessID);
                }

                PetraPrincipal.ProcessID = AProcessID;
                AProcessID = 0;
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return PetraPrincipal;
        }

        #region Resourcestrings

        private static readonly string StrSystemDisabled1 = Catalog.GetString("OpenPetra is currently disabled due to {0}.");

        private static readonly string StrSystemDisabled2 = Catalog.GetString("It will be available on {0} at {1}.");

        private static readonly string StrSystemDisabled2Admin = Catalog.GetString("Proceed with caution.");

        private static readonly string StrUserIsRetired = Catalog.GetString("User is retired.");

        private static readonly string StrInvalidUserIDPassword = Catalog.GetString("Invalid User ID/Password.");

        #endregion

        /// <summary>
        /// create SHA1 hash of password and the salt.
        /// replacement for FormsAuthentication.HashPasswordForStoringInConfigFile
        /// which is part of System.Web.dll and not available in the client profile of .net v4.0
        /// </summary>
        /// <returns></returns>
        [NoRemoting]
        public static string CreateHashOfPassword(string APasswordAndSalt, string AHashType = "SHA1")
        {
            if (AHashType.ToUpper() == "MD5")
            {
                return BitConverter.ToString(
                    MD5.Create().
                    ComputeHash(Encoding.UTF8.GetBytes(APasswordAndSalt))).Replace("-", "");
            }

            // default to SHA1
            return BitConverter.ToString(
                SHA1.Create().
                ComputeHash(Encoding.UTF8.GetBytes(APasswordAndSalt))).Replace("-", "");
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
            Ict.Petra.Server.App.Core.DomainManager.ClientTaskAddToOtherClient(AUserID,
                SharedConstants.CLIENTTASKGROUP_USERINFOREFRESH,
                "",
                1);
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
        /// add a new user
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
        /// authenticate a user
        /// </summary>
        public IPrincipal PerformUserAuthentication(string AUserName, string APassword,
            out Int32 AProcessID,
            out Boolean ASystemEnabled)
        {
            return TUserManagerWebConnector.PerformUserAuthentication(AUserName, APassword, out AProcessID, out ASystemEnabled);
        }
    }
}