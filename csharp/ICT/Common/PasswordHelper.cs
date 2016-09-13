//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters, christiank
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
// along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>
//
using System;
using System.Security.Cryptography;
using System.Text;
using PasswordUtilities;
using Sodium;

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
            PasswordGenerator PWGenerator = new PasswordGenerator(new PasswordPolicy(15, 18));

            // NOTE: An Entropy of 91 was deemed sufficient in 2016 but it should be raised in future years to accommodate the increase
            // in computing power and hence the lessening of time it would take to break a password of such an Entropy.
            while (Entropy < 91)
            {
                PWGenerator.GeneratePassword();
                Entropy = PWGenerator.PasswordEntropy;
            }

            return PWGenerator.Password;
        }

        /// generate a new password salt
        public static string GetNewPasswordSalt()
        {
            byte[] Salt = PasswordHash.GenerateSalt();

            return Encoding.ASCII.GetString(Salt);
        }

        /// <summary>
        /// Generates a Password Hash using the 'Scrypt' Key Stretching Algorithm (which is provided through the libsodium-net
        /// libaray ['Sodium' namespace]).
        /// </summary>
        /// <remarks><em>IMPORTANT:</em> Changing the way 'Salt' is generated and/or changing the 'Password Hash Strength limit'
        /// that gets passed to the PasswordHash.ScryptHashBinary Method (currently PasswordHash.Strength.Medium)
        /// <em>***invalidates existing passwords***</em>, ie. users will no longer be able to log on with their passwords once
        /// this is done!
        /// <para />
        /// If those parameters should ever be changed then the Ict.Tools.PasswordResetter.exe application can be used to assign
        /// new, random passwords that are created 'in the new way' and users will need to log on once with that password,
        /// then they will immediately need to enter a new password. This will then be stored 'in the new way' and users will
        /// be able to log on as they used to do from then onwards - with the new password of their choice.</remarks>
        /// <param name="APassword">Password.</param>
        /// <param name="ASalt">Salt. Must have been created with <see cref="GetNewPasswordSalt"/>!!!</param>
        /// <returns>Password Hash created with the 'Scrypt' Key Stretching Algorithm.</returns>
        public static string GetPasswordHash(string APassword, string ASalt)
        {
            return BitConverter.ToString(
                PasswordHash.ScryptHashBinary(APassword, ASalt, PasswordHash.Strength.Medium)).Replace("-", "");
        }
    }
}
