//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Collections.Generic;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Shared.Interfaces.Plugins.MSysMan;
using Ict.Petra.Shared.MSysMan;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Shared.Security;
using Ict.Petra.Server.MSysMan.Security.UserManager.WebConnectors;
using GNU.Gettext;

namespace Plugin.AuthenticationPetraProgress
{
    /// <summary>
    /// Authenticate against the Petra 2.x (Progress) database
    /// to use this add to server config file: &lt;add key="UserAuthenticationMethod" value="Plugin.AuthenticationPetraProgress" /&gt;
    /// </summary>
    public class TUserAuthentication : IUserAuthentication
    {
        /// <summary>
        /// return true if the user is known and the password is correct;
        /// otherwise returns false and an error message
        /// </summary>
        public bool AuthenticateUser(string AUsername, string APassword, out string AMessage)
        {
            AMessage = "";

            TPetraPrincipal tempPrincipal;
            SUserRow UserDR = TUserManagerWebConnector.LoadUser(AUsername, out tempPrincipal);

            // Petra 2.x, s_password2_c, 16 characters only
            // set demo password:
            // . sqlexp.sh "update pub.s_user set s_password2_c = 'FE01CE2A7FBAC8FA' where s_user_id_c = 'TIMOP'"
            string password2 = UserDR["s_password2_c"].ToString();

            if ((UserDR != null) && (TUserManagerWebConnector.CreateHashOfPassword(APassword, "MD5").Substring(0, 16) != password2))
            {
                // todo: increase failed logins
                throw new EPasswordWrongException(Catalog.GetString("Invalid User ID/Password."));
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// this allows the system administrator to change the password of the user. sets password2 in the Progress database
        /// </summary>
        public bool SetPassword(string AUsername, string APassword)
        {
            TPetraPrincipal tempPrincipal;
            SUserRow UserDR = TUserManagerWebConnector.LoadUser(AUsername, out tempPrincipal);

            if (UserDR != null)
            {
                string NewPasswordHash = TUserManagerWebConnector.CreateHashOfPassword(APassword, "MD5").Substring(0, 16);

                TDBTransaction t = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

                List <OdbcParameter>parameters = new List <OdbcParameter>();
                OdbcParameter parameter = new OdbcParameter("userid", OdbcType.VarChar);
                parameter.Value = NewPasswordHash;
                parameters.Add(parameter);

                try
                {
                    DBAccess.GDBAccessObj.ExecuteNonQuery("UPDATE pub_s_user SET s_password2_c = ?", t, false, parameters.ToArray());
                }
                catch (Exception e)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    throw new Exception("Plugin.AuthenticationPetraProgress, SetPassword " + e.Message);
                }
                DBAccess.GDBAccessObj.CommitTransaction();
            }
            else
            {
                throw new Exception("Plugin.AuthenticationPetraProgress, SetPassword " + Catalog.GetString("Invalid User ID"));
            }

            return true;
        }

        /// <summary>
        /// this allows the user to change their own password. sets password2 in the Progress database
        /// </summary>
        public bool SetPassword(string AUsername, string APassword, string AOldPassword)
        {
            TPetraPrincipal tempPrincipal;
            SUserRow UserDR = TUserManagerWebConnector.LoadUser(AUsername, out tempPrincipal);

            if (UserDR != null)
            {
                try
                {
                    string Message;

                    if (!AuthenticateUser(AUsername, AOldPassword, out Message))
                    {
                        throw new Exception();
                    }
                }
                catch (Exception)
                {
                    TLogging.Log(
                        String.Format("Cannot change the password for user {0} because the old password is wrong",
                            AUsername));
                    return false;
                }

                string NewPasswordHash = TUserManagerWebConnector.CreateHashOfPassword(APassword, "MD5").Substring(0, 16);

                TDBTransaction t = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

                List <OdbcParameter>parameters = new List <OdbcParameter>();
                OdbcParameter parameter = new OdbcParameter("userid", OdbcType.VarChar);
                parameter.Value = NewPasswordHash;
                parameters.Add(parameter);

                try
                {
                    DBAccess.GDBAccessObj.ExecuteNonQuery("UPDATE pub_s_user SET s_password2_c = ?", t, false, parameters.ToArray());
                }
                catch (Exception e)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    throw new Exception("Plugin.AuthenticationPetraProgress, SetPassword " + e.Message);
                }
                DBAccess.GDBAccessObj.CommitTransaction();
            }
            else
            {
                throw new Exception("Plugin.AuthenticationPetraProgress, SetPassword " + Catalog.GetString("Invalid User ID"));
            }

            return true;
        }

        /// <summary>
        /// this will not be implemented
        /// </summary>
        public bool CreateUser(string AUsername, string APassword, string AFamilyName, string AFirstName)
        {
            return false;
        }

        /// <summary>
        /// which functionality is implemented by this dll
        /// </summary>
        public void GetAuthenticationFunctionality(out bool ACanCreateUser, out bool ACanChangePassword, out bool ACanChangePermissions)
        {
            ACanCreateUser = false;
            ACanChangePassword = true;
            ACanChangePermissions = false;
        }
    }
}