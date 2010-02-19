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
using System.Web.Security;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.Plugins.MSysMan;
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
    }
}