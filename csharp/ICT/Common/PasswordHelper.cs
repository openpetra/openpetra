//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Collections.Generic;
using PasswordUtilities;

namespace Ict.Common
{
    /// <summary>
    /// Interface which all Password Hashing Scheme implementations need to implement.
    /// </summary>
    public interface IPasswordHashingScheme
    {
        /// <summary>
        /// Generates a new secure random password.
        /// </summary>
        /// <param name="APassword">Secure random password.</param>
        /// <param name="ASalt">The Salt that was used in the creation of the Password Hash.</param>
        /// <param name="APasswordHash">Password Hash.</param>
        void GetNewPasswordSaltAndHash(out string APassword, out string ASalt, out string APasswordHash);

        /// <summary>
        /// Generates a new password Salt.
        /// </summary>
        /// <returns>New password Salt as a byte array.</returns>
        byte[] GetNewPasswordSalt();

        /// <summary>
        /// Generates a Password Hash using the Hash Algorithm which is provided through the password hashing
        /// methodology that is coded up in each Password Hashing Scheme implementation.
        /// </summary>
        string GetPasswordHash(string APassword, byte[] ASalt);
    }

    /// <summary>
    /// Helper Class for dealing with the secure hashing of passwords in OpenPetra.
    /// </summary>
    /// <remarks>OpenPetra supports different versions of password hashing algorithms; these are implemented in the
    /// Classes that implement the <see cref="IPasswordHashingScheme"/> Interface!</remarks>
    public static class TPasswordHelper
    {
        /// <summary>
        /// This is the password for the IUSROPEMAIL user.  If authentication is required by the EMail server so that clients can send emails from
        /// connections on the public internet, we can tell the client to authenticate using these credentials.
        /// That way they do not need to supply their own login credentials which we would have to store somewhere.
        /// The sysadmin for the servers needs to create this user with low privileges accessible by the mail server (locally or using Active Directory).
        /// The password must be set to 'never expires' and 'cannot be changed'.
        /// Note that the password is not stored in this file as text and it is never exposed to a client.
        /// Password is ...
        /// </summary>
        public readonly static byte[] EmailUserPassword = new byte[] {
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF
        };


        /// <summary>
        /// Number of the Password Hashing Scheme that represents OpenPetra's current Password Hashing Scheme.
        /// </summary>
        public static int CurrentPasswordSchemeNumber
        {
            get
            {
                return TPasswordHelper.GetPasswordSchemeVersionNumber(TPasswordHelper.CurrentPasswordScheme);
            }
        }

        /// <summary>
        /// *New* instance of the Class that implements OpenPetra's current Password Hashing Scheme ('Factory Pattern').
        /// </summary>
        /// <remarks>
        /// IMPORTANT: This Method defines what is regarded as OpenPetra's current Password Hashing Scheme!!!
        /// </remarks>
        public static IPasswordHashingScheme CurrentPasswordScheme
        {
            get
            {
                return new TPasswordHashingScheme_V2();
            }
        }

        /// <summary>
        /// *New* instance of the Class that implements the specified version of OpenPetra's Password Hashing Scheme
        /// ('Factory Pattern').
        /// </summary>
        public static IPasswordHashingScheme GetPasswordSchemeHelperForVersion(int AVersion)
        {
            switch (AVersion)
            {
                case 1:
                    return new TPasswordHashingScheme_V1();

                case 2:
                    return new TPasswordHashingScheme_V2();

                case 3:
                    return new TPasswordHashingScheme_V3();

                case 4:
                    return new TPasswordHashingScheme_V4();

                default:
                    throw new ArgumentException("Unsupported AVersion argument value '" +
                    AVersion + "'; supported versions are 1, 2, 3 and 4 (at present)", "AVersion");
            }
        }

        /// <summary>
        /// Version number of the Password Hashing Scheme instance passed in with <paramref name="APasswordHelperInstance"/>.
        /// </summary>
        /// <param name="APasswordHelperInstance">Instance of a Class that inherits from <see cref="IPasswordHashingScheme"/>.</param>
        /// <returns>The version number of the Password Hashing Scheme instance passed in with
        /// <paramref name="APasswordHelperInstance"/></returns>
        private static int GetPasswordSchemeVersionNumber(IPasswordHashingScheme APasswordHelperInstance)
        {
            string PasswordHelperName = APasswordHelperInstance.GetType().Name;
            int UnderscoreVPosition = PasswordHelperName.LastIndexOf("_V");

            return Convert.ToInt32(
                PasswordHelperName.Substring(UnderscoreVPosition + 2));
        }

        #region Helper Methods

        /// <summary>
        /// Generates a new secure password.
        /// </summary>
        /// <returns>New secure password.</returns>
        public static string GetRandomSecurePassword()
        {
            double Entropy = 0;
            PasswordPolicy PWPolicy = new PasswordPolicy(15, 18);

            // Don't allow the following characters:     | *$£-+_&=%/\^~#@
            // (because the 'british pound' character doesn't exist on every keyboard!)
            PWPolicy.CharacterSetRemove("AO");

            PasswordGenerator PWGenerator = new PasswordGenerator(PWPolicy);

            // NOTE: An Entropy of 91 was deemed sufficient in 2016 but it should be raised in future years to accommodate
            // the increase in computing power (CPU and GPU) and advance of parallel processing and hence the lessening of
            // time it would take to break a password of such an Entropy.
            while (Entropy < 91)
            {
                PWGenerator.GeneratePassword();
                Entropy = PWGenerator.PasswordEntropy;
            }

            return PWGenerator.Password;
        }

        /// <summary>
        /// Logs the decimal representation of a byte array of arbitrary length for debugging purposes.
        /// </summary>
        /// <param name="AArray">Byte array.</param>
        /// <param name="ADescription">Description of the byte array.</param>
        public static void LogByteArrayContents(byte[] AArray, string ADescription)
        {
            string LogMessage = String.Format("Byte representation of passed-in byte array '{0}':  ", ADescription);

            for (int i = 0; i < AArray.Length; i++)
            {
                LogMessage += String.Format("B.{0}: {1}; ", i, AArray[i]);
            }

            TLogging.Log(LogMessage);
        }

        /// <summary>
        /// Compare two byte arrays.
        /// Avoiding timing attacks by making sure that the comparison always takes the same amount of time.
        /// </summary>
        /// <param name="a">array 1.</param>
        /// <param name="b">array 2.</param>
        /// <returns>True if equal. Otherwise False.</returns>
        public static bool EqualsAntiTimingAttack(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;

            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
               diff |= (uint)(a[i] ^ b[i]);
            }

            return diff == 0;
        }

        #endregion
    }
}
