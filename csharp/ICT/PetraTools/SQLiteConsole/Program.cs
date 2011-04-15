//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
// along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
//
using System;
using System.Data;
using Ict.Common;
using Ict.Common.DB;

namespace SQLiteConsole
{
/// <summary>
/// the problem is that there are no admin tools to look at an encrypted SQLite file
/// this is a console tool that will help analyzing the content of an encrypted sqlite file,
/// using the same encryption as the OpenPetra program.
/// </summary>
class TSQLiteConsole
{
    private static TDataBase db;
    private static TAppSettingsManager settings;

    // establish connection to database
    public static bool InitDBConnection(
        string ADBFile, string ADBPassword)
    {
        db = new TDataBase();

        new TLogging("debug.log");
        TLogging.DebugLevel = settings.GetInt16("DebugLevel", 0);

        db.EstablishDBConnection(TDBType.SQLite,
            ADBFile,
            "",
            "",
            "",
            ADBPassword,
            "");
        DBAccess.GDBAccessObj = db;

        return true;
    }

    public static void Main(string[] args)
    {
        settings = new TAppSettingsManager(false);

        if (!settings.HasValue("file"))
        {
            Console.WriteLine("call: echo SELECT * FROM s_user | SQLiteConsole -file:mydatabase.db -password:secret");
            Environment.Exit(1);
        }

        try
        {
            if (!System.IO.File.Exists(settings.GetValue("file")))
            {
                Console.WriteLine("database file " + settings.GetValue("file") + " does not exist");

                // this is to avoid InitDBConnection trying to find/copy the base database
                Environment.Exit(1);
            }

            if (!InitDBConnection(settings.GetValue("file"), settings.GetValue("password", "")))
            {
                Console.WriteLine("cannot connect to database " + settings.GetValue("file"));
                Environment.Exit(1);
            }

            string SQLCommand = "";
            string line;

            while ((line = Console.ReadLine()) != null && line.Length > 0)
            {
                SQLCommand += " " + line.Trim();
            }

            TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);
            DataTable result = DBAccess.GDBAccessObj.SelectDT(SQLCommand, "temp", transaction);
            TDataBase.LogTable(result);
            DBAccess.GDBAccessObj.RollbackTransaction();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
        }

        if (db.ConnectionOK)
        {
            db.CloseDBConnection();
        }
    }
}
}