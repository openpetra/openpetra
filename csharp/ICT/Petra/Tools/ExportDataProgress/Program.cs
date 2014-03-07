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
using System.Xml;
using System.IO;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.IO;

namespace Ict.Tools.ExportDataProgress
{
    class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                // establish connection to database
                TCmdOpts settings = new TCmdOpts();

                if (!settings.IsFlagSet("Server.ODBC_DSN"))
                {
                    Console.WriteLine(
                        "sample call: " +
                        "ExportDataProgress.exe -Server.ODBC_DSN:Petra2_2sa -username:demo_sql -password:demo -sql:\"SELECT * from pub.a_account\" -output:test.xml");
                    Environment.Exit(-1);
                }

                new TLogging("debug.log");

                TDataBase db = new TDataBase();

                TDBType dbtype = TDBType.ProgressODBC;

                if (settings.IsFlagSet("Server.RDBMSType"))
                {
                    dbtype = CommonTypes.ParseDBType(settings.GetOptValue("Server.RDBMSType"));
                }

                if (dbtype != TDBType.ProgressODBC)
                {
                    throw new Exception("at the moment only Progress ODBC db is supported");
                }

                db.EstablishDBConnection(dbtype,
                    settings.GetOptValue("Server.ODBC_DSN"),
                    "",
                    "",
                    settings.GetOptValue("username"),
                    settings.GetOptValue("password"),
                    "");
                DBAccess.GDBAccessObj = db;

                TLogging.DebugLevel = 10;

                string sqlText = "";

                if (!settings.IsFlagSet("sql"))
                {
                    Console.WriteLine("Please enter sql and finish with semicolon: ");

                    while (!sqlText.Trim().EndsWith(";"))
                    {
                        sqlText += " " + Console.ReadLine();
                    }

                    sqlText = sqlText.Substring(0, sqlText.Length - 1);
                }
                else
                {
                    sqlText = settings.GetOptValue("sql");
                    Console.WriteLine(sqlText);
                }

                TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadUncommitted);

                DataTable table = db.SelectDT(sqlText, "temp", transaction);

                XmlDocument doc = TDataBase.DataTableToXml(table);

                if (settings.IsFlagSet("output"))
                {
                    if (settings.GetOptValue("output").EndsWith("yml"))
                    {
                        TYml2Xml.Xml2Yml(doc, settings.GetOptValue("output"));
                    }
                    else if (settings.GetOptValue("output").EndsWith("csv"))
                    {
                        TCsv2Xml.Xml2Csv(doc, settings.GetOptValue("output"));
                    }
                    else if (settings.GetOptValue("output").EndsWith("xml"))
                    {
                        StreamWriter sw = new StreamWriter(settings.GetOptValue("output"));
                        sw.Write(TXMLParser.XmlToString2(doc));
                        sw.Close();
                    }
                }
                else
                {
                    TYml2Xml.Xml2Yml(doc, "temp.yml");
                    StreamReader sr = new StreamReader("temp.yml");
                    Console.WriteLine(sr.ReadToEnd());
                    sr.Close();
                }

                db.RollbackTransaction();
                db.CloseDBConnection();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}