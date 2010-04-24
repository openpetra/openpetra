/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2010 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Data;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Web.Security;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.Plugins.MSysMan;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MSysMan.Data.Access;
using Ict.Petra.Shared.Security;
using Ict.Petra.Server.App.Core.Security;

namespace Ict.Petra.Server.MSysMan.Maintenance.WebConnectors
{
    /// <summary>
    /// maintain the system, eg. user management etc
    /// </summary>
    public class TMaintenanceWebConnector
    {
        /// <summary>
        /// set the password of an existing user. this takes into consideration how users are authenticated in this system, by
        /// using an optional authentication plugin dll
        /// </summary>
        public static bool SetUserPassword(string AUsername, string APassword)
        {
            // TODO: check permissions. is the current user allowed to change the password of other users?

            string UserAuthenticationMethod = TAppSettingsManager.GetValueStatic("UserAuthenticationMethod", "OpenPetraDBSUser");

            if (UserAuthenticationMethod == "OpenPetraDBSUser")
            {
                TPetraPrincipal tempPrincipal;
                SUserRow UserDR = TUserManager.LoadUser(AUsername.ToUpper(), out tempPrincipal);
                SUserTable UserTable = (SUserTable)UserDR.Table;

                UserDR.PasswordHash = FormsAuthentication.HashPasswordForStoringInConfigFile(String.Concat(APassword,
                        UserDR.PasswordSalt), "SHA1");

                TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
                TVerificationResultCollection VerificationResult;
                SUserAccess.SubmitChanges(UserTable, Transaction, out VerificationResult);

                DBAccess.GDBAccessObj.CommitTransaction();

                return true;
            }
            else
            {
                // namespace of the class TUserAuthentication, eg. Plugin.AuthenticationPhpBB
                // the dll has to be in the normal application directory
                string Namespace = UserAuthenticationMethod;
                string NameOfDll = Namespace + ".dll";
                string NameOfClass = Namespace + ".TUserAuthentication";

                // dynamic loading of dll
                System.Reflection.Assembly assemblyToUse = System.Reflection.Assembly.LoadFrom(NameOfDll);
                System.Type CustomClass = assemblyToUse.GetType(NameOfClass);

                IUserAuthentication auth = (IUserAuthentication)Activator.CreateInstance(CustomClass);

                return auth.SetPassword(AUsername, APassword);
            }
        }

        /// <summary>
        /// creates a user, either using the default authentication with the database or with the optional authentication plugin dll
        /// </summary>
        public static bool CreateUser(string AUsername, string APassword, string AModulePermissions)
        {
            // TODO: check permissions. is the current user allowed to create other users?
            // TODO: fail on situation that user exists already, etc.

            SUserTable userTable = new SUserTable();
            SUserRow newUser = userTable.NewRowTyped();

            newUser.UserId = AUsername.ToUpper();
            userTable.Rows.Add(newUser);

            string UserAuthenticationMethod = TAppSettingsManager.GetValueStatic("UserAuthenticationMethod", "OpenPetraDBSUser");

            if (UserAuthenticationMethod == "OpenPetraDBSUser")
            {
                const int SALTSIZE = 32;
                byte[] saltBytes = new byte[SALTSIZE];
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                rng.GetNonZeroBytes(saltBytes);
                newUser.PasswordSalt = Convert.ToBase64String(saltBytes);
                newUser.PasswordHash = FormsAuthentication.HashPasswordForStoringInConfigFile(String.Concat(APassword,
                        newUser.PasswordSalt), "SHA1");
            }
            else
            {
                // namespace of the class TUserAuthentication, eg. Plugin.AuthenticationPhpBB
                // the dll has to be in the normal application directory
                string Namespace = UserAuthenticationMethod;
                string NameOfDll = Namespace + ".dll";
                string NameOfClass = Namespace + ".TUserAuthentication";

                // dynamic loading of dll
                System.Reflection.Assembly assemblyToUse = System.Reflection.Assembly.LoadFrom(NameOfDll);
                System.Type CustomClass = assemblyToUse.GetType(NameOfClass);

                IUserAuthentication auth = (IUserAuthentication)Activator.CreateInstance(CustomClass);

                if (!auth.CreateUser(AUsername, APassword))
                {
                    newUser = null;
                }
            }

            if (newUser != null)
            {
                TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
                TVerificationResultCollection VerificationResult;

                if (!SUserAccess.SubmitChanges(userTable, Transaction, out VerificationResult))
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    return false;
                }

                // TODO: set permissions. for the moment, create all permissions
                List <string>modules = new List <string>();
                modules.Add("PTNRUSER");
                modules.Add("FINANCE-1");
                modules.Add("SYSMAN");

                ALedgerTable theLedgers = ALedgerAccess.LoadAll(Transaction);

                foreach (ALedgerRow ledger in theLedgers.Rows)
                {
                    modules.Add("LEDGER" + ledger.LedgerNumber.ToString("0000"));
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

                if (!SUserModuleAccessPermissionAccess.SubmitChanges(moduleAccessPermissionTable, Transaction, out VerificationResult))
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    return false;
                }

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

                if (!SUserTableAccessPermissionAccess.SubmitChanges(tableAccessPermissionTable, Transaction, out VerificationResult))
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    return false;
                }

                DBAccess.GDBAccessObj.CommitTransaction();

                return true;
            }

            return false;
        }

        /// <summary>
        /// this is called from the MaintainUsers screen, for adding users, retiring users, set the password, etc
        /// </summary>
        public TSubmitChangesResult SaveSUser(ref MaintainUsersTDS ASubmitDS, out TVerificationResultCollection AVerificationResult)
        {
        }
    }
}