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

namespace GenerateEncryptionKey
{
class Program
{
    public static void Main(string[] args)
    {
        try
        {
            string keyfile = "../../../Testing/secretkey.txt";
            Rijndael alg = new RijndaelManaged();
            alg.GenerateKey();
            FileStream fs = new FileStream(keyfile, FileMode.CreateNew | FileMode.Create);
            fs.Write(alg.Key, 0, alg.Key.Length);
            fs.Close();
            Console.WriteLine("new key has been written to " + Path.GetFullPath(keyfile));
        }
        catch (Exception e)
        {
            Console.WriteLine("error: " + e.Message);
        }

        //Console.ReadLine();
    }
}
}