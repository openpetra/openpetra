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
using System.Collections.Generic;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Shared.Interfaces.Plugins.MSysMan;
using GNU.Gettext;

namespace Plugin.AuthenticationPhpBB
{
    /// <summary>
    /// Authenticate against phpBB forum via php script;
    /// to use this add to server config file: &lt;add key="UserAuthenticationMethod" value="Plugin.AuthenticationPhpBB" /&gt;
    /// also set UserAuthenticationMethod.Url in the config file, and upload authenticate.php to your phpBB forum and your secret key file as well
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
            string keyfile = TAppSettingsManager.GetValue("Server.ChannelEncryption.Keyfile", "");
            byte[] key = EncryptionRijndael.ReadSecretKey(keyfile);
            string encryptedMessage;
            string ivMessage;
            EncryptionRijndael.Encrypt(key, AUsername + ";" + APassword + ";", out encryptedMessage, out ivMessage);

            // url is for example: http://forum.example.org/OpenPetraLogin/authenticate.php
            string url = TAppSettingsManager.GetValue("UserAuthenticationMethod.Url", "");
            SortedList <string, string>parameters = new SortedList <string, string>();
            parameters.Add("msg", encryptedMessage);
            parameters.Add("msg2", ivMessage);
            string resultWebsite = THTTPUtils.ReadWebsite(url, parameters);
            try
            {
                string result = EncryptionRijndael.Decrypt(key,
                    resultWebsite,
                    ivMessage);

                if (result.StartsWith("LOGIN_SUCCESS"))
                {
                    return true;
                }

                if (result.StartsWith("LOGIN_ERROR_ATTEMPTS"))
                {
                    AMessage =
                        Catalog.GetString(
                            "Your account has been disabled because you typed the wrong password too often.\n" +
                            "Please login to the forum again, you will have to solve a captcha to verify that you are you. You can also reset your password there.");
                    return false;
                }

                AMessage = Catalog.GetString("Invalid User ID or Password.");
            }
            catch (Exception e)
            {
                TLogging.Log(e.Message, TLoggingType.ToLogfile);
                TLogging.Log(e.StackTrace, TLoggingType.ToLogfile);
            }

            return false;
        }

        /// <summary>
        /// not implemented here. change the password in the forum
        /// </summary>
        public bool SetPassword(string AUsername, string APassword)
        {
            return false;
        }

        /// <summary>
        /// not implemented here. change the password in the forum
        /// </summary>
        public bool SetPassword(string AUsername, string APassword, string AOldPassword)
        {
            return false;
        }

        /// <summary>
        /// could check if the user already exists in phpBB
        /// </summary>
        public bool CreateUser(string AUsername, string APassword, string AFamilyName, string AFirstName)
        {
            // TODO check if user exists. careful: password is probably not valid, since the sysadmin does not know it
            return true;
        }

        /// <summary>
        /// which functionality is implemented by this dll
        /// </summary>
        public void GetAuthenticationFunctionality(out bool ACanCreateUser, out bool ACanChangePassword, out bool ACanChangePermissions)
        {
            ACanCreateUser = true;
            ACanChangePassword = false;
            ACanChangePermissions = true;
        }
    }
}