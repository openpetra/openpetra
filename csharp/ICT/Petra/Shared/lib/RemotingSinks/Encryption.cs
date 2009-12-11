/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
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
using System.IO;
using System.Security.Cryptography;

namespace Ict.Petra.Shared.RemotingSinks.Encryption
{
    /// <summary>
    /// this uses the Rijndael encryption, with a secret key that boths parties know
    /// </summary>
    public class EncryptionHelper
    {
        /// <summary>
        /// identify the used encryption
        /// </summary>
        public static string ENCRYPTIONNAME = "Rijndael";

        private byte[] FEncryptionKey;

        /// <summary>
        /// constructor
        /// </summary>
        public EncryptionHelper(byte[] AEncryptionKey)
        {
            this.FEncryptionKey = AEncryptionKey;
        }

        /// <summary>
        /// Encrypt a stream, create an initialization vector, which is transmitted with the message
        /// </summary>
        /// <param name="source"></param>
        /// <param name="AEncryptionIV"></param>
        /// <returns></returns>
        public Stream Encrypt(Stream source, out byte[] AEncryptionIV)
        {
            Stream outStream = new System.IO.MemoryStream();

            // setup the encryption properties
            SymmetricAlgorithm alg = SymmetricAlgorithm.Create(ENCRYPTIONNAME);

            alg.Key = FEncryptionKey;
            alg.GenerateIV();
            AEncryptionIV = alg.IV;

            CryptoStream encryptStream = new CryptoStream(
                outStream,
                alg.CreateEncryptor(),
                CryptoStreamMode.Write);

            // write the whole contents through the new streams
            byte[] buf = new Byte[1000];
            int cnt = source.Read(buf, 0, 1000);

            while (cnt > 0)
            {
                encryptStream.Write(buf, 0, cnt);
                cnt = source.Read(buf, 0, 1000);
            }

            encryptStream.FlushFinalBlock();
            outStream.Seek(0, SeekOrigin.Begin);
            return outStream;
        }

        /// Decrypt a stream, using the initialization vector, which was transmitted with the message
        public Stream Decrypt(Stream source, byte[] AEncryptionIV)
        {
            Stream outStream = new System.IO.MemoryStream();

            // setup decryption properties
            SymmetricAlgorithm alg = SymmetricAlgorithm.Create(ENCRYPTIONNAME);

            alg.Key = FEncryptionKey;
            alg.IV = AEncryptionIV;

            // add the decryptor layer to the stream
            CryptoStream decryptStream = new CryptoStream(
                source,
                alg.CreateDecryptor(),
                CryptoStreamMode.Read);

            // write the whole contents through the new streams
            byte[] buf = new Byte[1000];
            int cnt = decryptStream.Read(buf, 0, 1000);

            while (cnt > 0)
            {
                outStream.Write(buf, 0, cnt);
                cnt = decryptStream.Read(buf, 0, 1000);
            }

            outStream.Flush();
            outStream.Seek(0, SeekOrigin.Begin);

            return outStream;
        }
    }
}