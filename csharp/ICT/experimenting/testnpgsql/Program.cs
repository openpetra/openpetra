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
using Ict.Common;
using Ict.Common.DB;
using Npgsql;

namespace testnpgsql
{
class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");
        new TLogging("temp.log");

        try
        {
#if EXTREME_DEBUGGING
            NpgsqlEventLog.Level = LogLevel.Debug;
            NpgsqlEventLog.LogName = "NpgsqlTests.LogFile";
            NpgsqlEventLog.EchoMessages = true;
#endif

            DBAccess.GDBAccessObj = new TDataBase();
            DBAccess.GDBAccessObj.EstablishDBConnection(TDBType.PostgreSQL, "localhost", "5432", "petraserver", "TOBESETBYINSTALLER", "");

            TDBTransaction tr = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_s_user_defaults WHERE s_user_id_c = 'DEMO' AND s_default_code_c = 'TESTNPGSQL'",
                tr,
                false);
            DBAccess.GDBAccessObj.CommitTransaction();

            tr = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);
            DBAccess.GDBAccessObj.ExecuteNonQuery(
                "INSERT INTO PUB_s_user_defaults(s_user_id_c, s_default_code_c, s_default_value_c) values ('DEMO', 'TESTNPGSQL', 'test')",
                tr,
                false);
            DBAccess.GDBAccessObj.ExecuteNonQuery(
                "INSERT INTO PUB_s_user_defaults(s_user_id_c, s_default_code_c, s_default_value_c) values ('DEMO', 'TESTNPGSQL2', 'test')",
                tr,
                false);
            DBAccess.GDBAccessObj.RollbackTransaction();

            tr = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);
            Console.WriteLine(" should be 0: " +
                DBAccess.GDBAccessObj.ExecuteScalar(
                    "SELECT COUNT(*) FROM PUB_s_user_defaults WHERE s_user_id_c = 'DEMO' AND s_default_code_c = 'TESTNPGSQL'",
                    tr, false).ToString());
            DBAccess.GDBAccessObj.RollbackTransaction();

            tr = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);
            DBAccess.GDBAccessObj.ExecuteNonQuery(
                "INSERT INTO PUB_s_user_defaults(s_user_id_c, s_default_code_c, s_default_value_c) values ('DEMO', 'TESTNPGSQL', 'test')",
                tr,
                false);
            DBAccess.GDBAccessObj.CommitTransaction();

            tr = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);
            Console.WriteLine(" should be 1: " +
                DBAccess.GDBAccessObj.ExecuteScalar(
                    "SELECT COUNT(*) FROM PUB_s_user_defaults WHERE s_user_id_c = 'DEMO' AND s_default_code_c = 'TESTNPGSQL'",
                    tr, false).ToString());
            DBAccess.GDBAccessObj.RollbackTransaction();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        Console.Write("Press any key to continue . . . ");
        Console.ReadKey(true);
    }
}
}