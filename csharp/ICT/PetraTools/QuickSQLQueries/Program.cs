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
using System.Data;
using System.IO;
using Ict.Common;
using Ict.Common.DB;

namespace QuickSQLTests
{
class Program
{
    /// establish connection to database
    public static bool InitDBConnection()
    {
        TDataBase db = new TDataBase();

        new TLogging("debug.log");
        db.DebugLevel = Convert.ToInt16(TAppSettingsManager.GetValueStatic("DebugLevel", "0"));

        StreamReader sr = new StreamReader("u:\\secret.txt");

        db.EstablishDBConnection(TDBType.ProgressODBC,
            sr.ReadLine(),                     // DSN
            "",
            "",
            sr.ReadLine(),         // username
            sr.ReadLine(),         // password
            "");
        DBAccess.GDBAccessObj = db;

        sr.Close();

        return true;
    }

    public static void Main(string[] args)
    {
        try
        {
            InitDBConnection();

            TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            GetMissingLinkedPartnersCosCentre.Run(transaction);

            DBAccess.GDBAccessObj.RollbackTransaction();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
        }
        Console.Write("Press any key to continue . . . ");
        Console.ReadKey(true);
    }
}
}