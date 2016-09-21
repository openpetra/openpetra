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
using System.Text;

using Sodium;

namespace Ict.Common
{
    /// <summary>
    /// Password Hashing Scheme V2: Generates 'salted' Password Hashes with the Scrypt Key Stretching Algorithm.
    /// </summary>
    /// <remarks>
    /// The Scrypt hash function is used through the libsodium-net library.
    /// The 'password hash strength' is set to <see cref="PasswordHash.Strength.Medium"/>.
    /// IMPORTANT: The name of the Class MUST end with '_V' followed by 2 (gets evaluated in Method
    /// 'GetPasswordSchemeVersionNumber' of Class TPasswordHelper)!!!
    /// </remarks>
    internal class TPasswordHashingScheme_V2 : IPasswordHashingScheme
    {
        /// <summary>
        /// Generates a new secure random password.
        /// </summary>
        /// <param name="APassword">Secure random password.</param>
        /// <param name="ASalt">The Salt that was used in the creation of the Password Hash.</param>
        /// <param name="APasswordHash">Password Hash.</param>
        public void GetNewPasswordSaltAndHash(out string APassword, out string ASalt, out string APasswordHash)
        {
            byte[] Salt = GetNewPasswordSalt();

            APassword = TPasswordHelper.GetRandomSecurePassword();
            APasswordHash = GetPasswordHash(APassword, Salt);
            ASalt = Convert.ToBase64String(Salt);
        }

        /// <summary>
        /// Generates a new password Salt.
        /// </summary>
        /// <returns>New password Salt as a byte array.</returns>
        public byte[] GetNewPasswordSalt()
        {
            byte[] Salt = PasswordHash.GenerateSalt();

            //TPasswordHelper.LogByteArrayContents(Salt, "TPasswordHashingScheme_V2, _V3 and _V4 - Salt");

            return Salt;
        }

        /// <summary>
        /// Generates a Password Hash using the 'Scrypt' Key Stretching Algorithm (which is provided through the libsodium-net
        /// libaray ['Sodium' namespace]).
        /// </summary>
        /// <remarks><em>IMPORTANT:</em> Changing the way 'Salt' is generated and/or changing the 'Password Hash Strength limit'
        /// that gets passed to the PasswordHash.ScryptHashBinary Method (here: PasswordHash.Strength.Medium)
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
        public virtual string GetPasswordHash(string APassword, byte[] ASalt)
        {
            byte[] Hash =
                PasswordHash.ScryptHashBinary(Encoding.Unicode.GetBytes(APassword), ASalt, PasswordHash.Strength.Medium);

            //TPasswordHelper.LogByteArrayContents(Hash, "TPasswordHashingScheme_V2 - Password Hash");

            return Convert.ToBase64String(Hash);
        }
    }
}