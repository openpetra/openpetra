//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
//
// Copyright 2004-2010 by OM International
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
// along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>
//
using System;
using System.Security.Cryptography;
using System.Text;
using PasswordUtilities;

namespace Ict.Common
{
    /// <summary>
    /// TODO
    /// </summary>
    public class PasswordHelper
    {
        /// <summary>
        /// This is the password for the IUSROPEMAIL user.  If authentication is required by the EMail server so that clients can send emails from
        /// connections on the public internet, we can tell the client to authenticate using these credentials.
        /// That way they do not need to supply their own login credentials which we would have to store somewhere.
        /// The sysadmin for the servers needs to create this user with low privileges accessible by the mail server (locally or using Active Directory).
        /// The password must be set to 'never expires' and 'cannot be changed'.
        /// Note that the password is not stored in this file as text and it is never exposed to a client.
        // Password is ....
        public readonly static byte[] EmailUserPassword = new byte[] {
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF
        };

        /// <summary>
        /// Generate a new secure random password and salt. Use them to create a hash.
        /// </summary>
        /// <param name="APassword"></param>
        /// <param name="ASalt"></param>
        /// <param name="APasswordHash"></param>
        public static void GetNewPasswordSaltAndHash(out string APassword, out string ASalt, out string APasswordHash)
        {
            APassword = GetRandomSecurePassword();
            ASalt = GetNewPasswordSalt();
            APasswordHash = GetPasswordHash(APassword, ASalt);
        }

        /// generate a new secure password
        public static string GetRandomSecurePassword()
        {
            double Entropy = 0;
            PasswordGenerator Generator = new PasswordGenerator();

            // TODO An Entropy of 72 was deemed sufficient in 2014 but it should be raised in future years to accommodate the increase
            // in computing power and hence the lessening of time it would take to break a password of such an Entropy.
            while (Entropy < 72)
            {
                Generator.GeneratePassword();
                Entropy = Generator.PasswordEntropy;
            }

            return Generator.Password;
        }

        /// generate a new password salt
        public static string GetNewPasswordSalt()
        {
            byte[] saltBytes = new byte[32];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(saltBytes);

            return Convert.ToBase64String(saltBytes);;
        }

        /// <summary>
        /// generate a password hash
        /// </summary>
        /// <param name="APassword"></param>
        /// <param name="ASalt"></param>
        /// <returns>Hash</returns>
        public static string GetPasswordHash(string APassword, string ASalt)
        {
            return GetPasswordHash(String.Concat(APassword, ASalt));
        }

        /// <summary>
        /// generate a password hash
        /// </summary>
        /// <param name="APasswordAndSalt"></param>
        /// <returns>Hash</returns>
        public static string GetPasswordHash(string APasswordAndSalt)
        {
            return BitConverter.ToString(SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(
                        APasswordAndSalt))).Replace("-", "");
        }
    }
}
