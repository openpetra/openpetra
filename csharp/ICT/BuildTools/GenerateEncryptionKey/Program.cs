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
using System.Security.Cryptography;
using Ict.Common;
using Ict.Common.IO;

namespace Ict.Tools.GenerateEncryptionKey
{
    class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                new TAppSettingsManager(false);
                CspParameters cp = new CspParameters();
                cp.KeyContainerName = "OpenPetraServerKeyContainer";

                // first make sure, we really get a new key
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(cp);
                RSA.PersistKeyInCsp = false;
                RSA.Clear();

                // now create the new key
                RSACryptoServiceProvider RSANew = new RSACryptoServiceProvider(cp);

                string PublicKeyFile = TAppSettingsManager.GetValue("PublicKeyFile", "", false);

                if (PublicKeyFile.Length > 0)
                {
                    StreamWriter sw = new StreamWriter(PublicKeyFile);
                    sw.WriteLine(RSANew.ToXmlString(false));
                    sw.Close();
                    Console.WriteLine("public key has been written to " + PublicKeyFile);
                }
                else
                {
                    Console.WriteLine("public key only: ");
                    Console.WriteLine(RSANew.ToXmlString(false));
                }

                string PrivateKeyFile = TAppSettingsManager.GetValue("PrivateKeyFile", "", false);

                if (PrivateKeyFile.Length > 0)
                {
                    StreamWriter sw = new StreamWriter(PrivateKeyFile);
                    sw.WriteLine(RSANew.ToXmlString(true));
                    sw.Close();
                    Console.WriteLine("private key has been written to " + PrivateKeyFile);
                }
                else
                {
                    Console.WriteLine("Private key with public key: ");
                    Console.WriteLine(RSANew.ToXmlString(true));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error: " + e.Message);
                Console.WriteLine("error: " + e.StackTrace);
            }
        }
    }
}