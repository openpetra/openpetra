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
using System.IO;
using GNU.Gettext;
using System.Security.Cryptography;

namespace Ict.Common.IO
{
    /// contains functions to deal with encryption and decryption;
    /// see also BuildTools/GenerateEncryptionKey, and Petra/Shared/RemotingSinks
    public class EncryptionRijndael
    {
        /// <summary>
        /// the key is stored as a base64 string to make it easier to handle the key;
        /// this method returns the clear version of the key to be used with encrypt and decrypt functions
        /// </summary>
        static public byte[] ReadSecretKey(string ASecretKeyFile)
        {
            StreamReader r = new StreamReader(ASecretKeyFile);
            string line = r.ReadToEnd();

            r.Close();
            return Convert.FromBase64String(line.Trim());
        }

        /// store a key into a text file
        static public bool CreateSecretKey(string ASecretKeyFile)
        {
            Rijndael alg = new RijndaelManaged();

            alg.GenerateKey();
            StreamWriter sw = new StreamWriter(ASecretKeyFile);
            sw.Write(Convert.ToBase64String(alg.Key));
            sw.Close();
            return true;
        }

        /// encrypt a string message using a secret key that is known to both sender and recipient only;
        /// need to give the initialization vector to the recipient as well;
        static public bool Encrypt(byte[] ASecretKey, string AMessage, out string AEncryptedMessage, out string AInitializationVector)
        {
            Rijndael alg = new RijndaelManaged();

            alg.Key = ASecretKey;

            alg.GenerateIV();

            MemoryStream ms = new MemoryStream();

            CryptoStream encryptStream = new CryptoStream(
                ms,
                alg.CreateEncryptor(),
                CryptoStreamMode.Write);

            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            byte[] toEncryptBytes = enc.GetBytes(AMessage);
            encryptStream.Write(toEncryptBytes, 0, toEncryptBytes.Length);
            encryptStream.Close();

            AEncryptedMessage = Convert.ToBase64String(ms.ToArray());
            AInitializationVector = Convert.ToBase64String(alg.IV);

            return true;
        }

        /// <summary>
        /// decrypt an encrypted message, using the secret key and the initialization vector that was used to encrypt the message
        /// </summary>
        static public string Decrypt(byte[] ASecretKey, string AEncryptedMessage, string AInitializationVector)
        {
            Rijndael alg = new RijndaelManaged();

            alg.Key = ASecretKey;
            alg.Padding = PaddingMode.Zeros;
            alg.IV = Convert.FromBase64String(AInitializationVector);
            MemoryStream ms = new MemoryStream();
            CryptoStream decryptStream = new CryptoStream(
                ms,
                alg.CreateDecryptor(),
                CryptoStreamMode.Write);
            byte[] toDecryptBytes = Convert.FromBase64String(AEncryptedMessage);
            decryptStream.Write(toDecryptBytes, 0, toDecryptBytes.Length);
            decryptStream.Close();
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            string decryptedMessage = enc.GetString((ms.ToArray()));

            return decryptedMessage;
        }

        /// <summary>
        /// Encrypt a stream, create an initialization vector, which is transmitted with the message
        /// </summary>
        static public Stream Encrypt(byte[] AEncryptionKey, Stream source, out byte[] AEncryptionIV)
        {
            Stream outStream = new System.IO.MemoryStream();

            // setup the encryption properties
            Rijndael alg = new RijndaelManaged();

            alg.Key = AEncryptionKey;
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
        static public Stream Decrypt(byte[] AEncryptionKey, Stream source, byte[] AEncryptionIV)
        {
            Stream outStream = new System.IO.MemoryStream();

            // setup decryption properties
            Rijndael alg = new RijndaelManaged();

            alg.Key = AEncryptionKey;
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

        /// <summary>
        /// the name of the encryption algorithm
        /// </summary>
        static public string GetEncryptionName()
        {
            return "RIJNDAEL";
        }
    }
}