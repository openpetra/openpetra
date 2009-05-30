/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       morayh
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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Ict.Petra.Shared.Security
{
    /// Implements data security
    public class TSecureData : object
    {
        /// private declarations
        private Encoding FEncoding;
        private Image FItem;
        private TripleDESCryptoServiceProvider TDES;
        private String[] FText;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="Icon"></param>
        public TSecureData(string[] Text, Image Icon) : base()
        {
            TDES = new TripleDESCryptoServiceProvider();
            FEncoding = new UTF8Encoding();
            FText = Text;
            FItem = Icon;
        }

        /// <summary>
        /// decrypt data
        /// </summary>
        /// <param name="Data">encrypted data</param>
        /// <returns>clear data</returns>
        public string GetData(string Data)
        {
            Byte[] KeyBlock = new Byte[4097];
            Byte[] KeyArray = new Byte[24];
            Byte[] IVArray = new Byte[8];

            MemoryStream OutputStream = new MemoryStream();
            I2KB(FItem, ref KeyBlock);
            GetKey(FText, KeyBlock, ref KeyArray, ref IVArray);
            Byte[] CryptArray = Convert.FromBase64String(Data);
            CryptoStream CryptStream = new CryptoStream(OutputStream, TDES.CreateDecryptor(KeyArray, IVArray), CryptoStreamMode.Write);
            CryptStream.Write(CryptArray, 0, CryptArray.Length);
            CryptStream.FlushFinalBlock();
            OutputStream.Seek(0, SeekOrigin.Begin);
            string ReturnValue = FEncoding.GetString(OutputStream.ToArray());
            CryptStream.Close();
            OutputStream.Close();
            return ReturnValue;
        }

        private void GetKey(string[] Salt, byte[] KeyBlock, ref byte[] Key, ref byte[] IV)
        {
            int Counter = 0;
            int Pos = 37;
            int SaltIndex = 0;
            int SaltItem = 0;

            do
            {
                Key[Counter] = KeyBlock[Pos];
                Counter = Counter + 1;
                Pos = Pos + (Byte)(Salt[SaltItem][SaltIndex]);

                if (SaltIndex == Salt[SaltItem].Length - 1)
                {
                    SaltIndex = 0;
                    SaltItem = SaltItem + 1;
                }
                else
                {
                    SaltIndex = SaltIndex + 1;
                }
            } while (Counter != Key.Length - 1);

            Key[Counter] = KeyBlock[Pos];

            for (Counter = 0; Counter < IV.Length; Counter += 1)
            {
                IV[Counter] = (Byte)(Key[Counter]);
            }
        }

        private void I2KB(Image Item, ref byte[] KeyBlock)
        {
            MemoryStream ImageStream;

            ImageStream = new MemoryStream();
            Item.Save(ImageStream, ImageFormat.Png);
            KeyBlock = ImageStream.ToArray();
            ImageStream.Close();
        }

        /// <summary>
        /// encrypt data
        /// </summary>
        /// <param name="Data">clear data</param>
        /// <returns>encrypted data</returns>
        public string PutData(string Data)
        {
            Byte[] KeyBlock = new Byte[4097];
            Byte[] KeyArray = new Byte[24];
            Byte[] IVArray = new Byte[8];

            I2KB(FItem, ref KeyBlock);
            GetKey(FText, KeyBlock, ref KeyArray, ref IVArray);
            MemoryStream OutputStream = new MemoryStream();
            CryptoStream CryptStream = new CryptoStream(OutputStream, TDES.CreateEncryptor(KeyArray, IVArray), CryptoStreamMode.Write);
            CryptStream.Write(FEncoding.GetBytes(Data), 0, FEncoding.GetByteCount(Data));
            CryptStream.FlushFinalBlock();
            OutputStream.Seek(0, SeekOrigin.Begin);
            string ReturnValue = Convert.ToBase64String(OutputStream.ToArray());
            CryptStream.Close();
            OutputStream.Close();
            return ReturnValue;
        }
    }
}