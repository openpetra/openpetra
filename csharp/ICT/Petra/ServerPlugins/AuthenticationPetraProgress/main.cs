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
using Ict.Petra.Shared.Interfaces.Plugins.MSysMan;
using Ict.Petra.Shared.MSysMan;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Shared.Security;
using Ict.Petra.Server.App.Core.Security;
using Mono.Unix;

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
            SUserRow UserDR = TUserManager.LoadUser(AUsername, out tempPrincipal);

            // Petra 2.x, s_password2_c, 16 characters only
            // set demo password:
            // . sqlexp.sh "update pub.s_user set s_password2_c = 'FE01CE2A7FBAC8FA' where s_user_id_c = 'TIMOP'"
            string password2 = UserDR["s_password2_c"].ToString();

            if ((UserDR != null) && (FormsAuthentication.HashPasswordForStoringInConfigFile(APassword, "MD5").Substring(0, 16) != password2))
            {
                // todo: increase failed logins
                throw new EPasswordWrongException(Catalog.GetString("Invalid User ID/Password."));
            }
            else
            {
                return true;
            }
        }
    }
}