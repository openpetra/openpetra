//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christiank
//
// Copyright 2004-2019 by OM International
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
using System.Collections.Generic;
using System.Web;
using System.Threading;
using System.Web.SessionState;
using System.Data;
using System.Data.Odbc;
using System.Linq;

using Mono.Data.Sqlite;

using Ict.Common;
using Ict.Common.DB;

using Newtonsoft.Json;

namespace Ict.Common.Session
{
    /// <summary>
    /// Static class for storing sessions.
    /// we are using our own session handling,
    /// since we want to store sessions in the database,
    /// and we want to run tests without HttpContext.
    /// </summary>
    public class TSession
    {
        // these variables are only used in the unit tests.
        // they are only used if HttpContext.Current == null.
        // they are used across threads, because we in the tests we want to access the session across threads.
        private static string FSessionID; // STATIC_OK: only needed for the tests
        private static SortedList <string, string> FSessionValues;  // STATIC_OK: only needed for the tests
        private static DateTime FSessionValidUntil;  // STATIC_OK: only needed for the tests

        private const int SessionValidHours = 24;

        private static SortedList <string, string> GetSessionValues(string ASessionID)
        {
            if (HttpContext.Current == null)
            {
                if ((FSessionID == ASessionID) && (FSessionValidUntil.CompareTo(DateTime.Now) > 0))
                {
                    return FSessionValues;
                }
            }
            else if ((HttpContext.Current.Session["SessionID"] != null) && (HttpContext.Current.Session["SessionID"].ToString() == ASessionID))
            {
                // we must get the session values each time from the database, for the reports' progress tracker to work.
                //return (SortedList <string, string>) HttpContext.Current.Session["SessionValues"];
                return null;
            }

            return null;
        }

        private static void SetSessionValues(string ASessionID, SortedList <string, string> AValues)
        {
            if (HttpContext.Current == null)
            {
                if ((FSessionID == ASessionID) || (FSessionID == String.Empty))
                {
                    FSessionValidUntil = DateTime.Now.AddHours(SessionValidHours);
                    FSessionID = ASessionID;
                    FSessionValues = AValues;
                }
            }
            else if (HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session["SessionID"] = ASessionID;
                HttpContext.Current.Session["SessionValues"] = AValues;
            }
        }

        private static void ClearSession()
        {
            if (HttpContext.Current == null)
            {
                FSessionValidUntil = DateTime.Now.AddHours(-1);
                FSessionID = String.Empty;
                FSessionValues = null;
            }
            else if (HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session.Clear();
            }
        }

        /// get the current session id. if it is not stored in the http context, check the thread
        private static string FindSessionID()
        {
            string sessionId;

            // only look in thread if there is no HttpContext.Current; otherwise Threads are reused.
            if (HttpContext.Current == null)
            {
                sessionId = FSessionID;

                if ((sessionId != null) && (sessionId.Length > 0))
                {
                    TLogging.LogAtLevel(4, "FindSessionID: Session ID found in thread. SessionID = " + sessionId);

                    return sessionId;
                }

                TLogging.LogAtLevel(1,
                    "FindSessionID: Session ID not found in the thread!!! thread id: " + Thread.CurrentThread.ManagedThreadId.ToString());
                return String.Empty;
            }
            else if (HttpContext.Current.Request.Cookies.AllKeys.Contains("OpenPetraSessionID"))
            {
                sessionId = HttpContext.Current.Request.Cookies["OpenPetraSessionID"].Value;
                if (sessionId != String.Empty)
                {
                    TLogging.LogAtLevel(4, "FindSessionID: Session ID found in HttpContext. SessionID = " + sessionId);
                    return sessionId;
                }
            }

            TLogging.LogAtLevel(1, "FindSessionID: Session ID not found in the HttpContext");
            return string.Empty;
        }

        /// <summary>
        /// set the session id for this current thread
        /// </summary>
        /// <param name="ASessionID"></param>
        public static void InitThread(string ASessionID)
        {
            TLogging.LogAtLevel(1, "Running InitThread for ASessionID = " + ASessionID);
            TLogging.LogAtLevel(1, "thread id " + Thread.CurrentThread.ManagedThreadId.ToString());

            if (HttpContext.Current == null)
            {
                FSessionID = ASessionID;
            }
        }

        /// establish a database connection to the alternative sqlite database for the sessions
        private static TDataBase EstablishDBConnectionSqliteSessionDB(String AConnectionName = "")
        {
            TDBType DBType = CommonTypes.ParseDBType(TAppSettingsManager.GetValue("Server.RDBMSType", "postgresql"));

            if (DBType != TDBType.SQLite)
            {
                throw new Exception("EstablishDBConnectionSqliteSessionDB: we should not get here.");
            }

            string DatabaseHostOrFile = TAppSettingsManager.GetValue("Server.DBSqliteSession", "localhost");
            string DatabasePort = String.Empty;
            string DatabaseName = TAppSettingsManager.GetValue("Server.DBName", "openpetra");
            string DBUsername = TAppSettingsManager.GetValue("Server.DBUserName", "petraserver");
            string DBPassword = TAppSettingsManager.GetValue("Server.DBPassword", string.Empty, false);

            if (!File.Exists(DatabaseHostOrFile))
            {
                // create the sessions database file
                TLogging.Log("create the sessions database file: " + DatabaseHostOrFile);

                // sqlite on Windows does not support encryption with a password
                // System.EntryPointNotFoundException: sqlite3_key
                DBPassword = string.Empty;

                SqliteConnection conn = new SqliteConnection("Data Source=" + DatabaseHostOrFile + (DBPassword.Length > 0 ? ";Password=" + DBPassword : ""));
                conn.Open();

                string createStmt = 
                    @"CREATE TABLE s_session (
                      s_session_id_c varchar(128) NOT NULL,
                      s_valid_until_d datetime NOT NULL,
                      s_session_values_c text,
                      s_date_created_d date,
                      s_created_by_c varchar(20),
                      s_date_modified_d date,
                      s_modified_by_c varchar(20),
                      s_modification_id_t timestamp,
                      CONSTRAINT s_session_pk
                        PRIMARY KEY (s_session_id_c)
                    )";

                SqliteCommand cmd = new SqliteCommand(createStmt, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            TDataBase DBAccessObj = new TDataBase();

            DBAccessObj.EstablishDBConnection(DBType,
                DatabaseHostOrFile,
                DatabasePort,
                DatabaseName,
                DBUsername,
                DBPassword,
                "",
                true,
                AConnectionName);

            return DBAccessObj;
        }

        private static TDataBase ConnectDB(string AConnectionName, TDataBase ADataBase, out bool ANewConnection)
        {
            // for SQLite, we use a different database for the session data, to avoid locking the database.
            if (DBAccess.DBType == TDBType.SQLite)
            {
                if (ADataBase != null)
                {
                    if (ADataBase.DsnOrServer == TAppSettingsManager.GetValue("Server.DBSqliteSession", "localhost"))
                    {
                        ANewConnection = false;
                        return ADataBase;
                    }
                }

                ANewConnection = true;

                return EstablishDBConnectionSqliteSessionDB(AConnectionName);
            }

            ANewConnection = (ADataBase == null);

            return DBAccess.Connect(AConnectionName, ADataBase);
        }

        private static bool HasValidSession(string ASessionID, TDataBase ADataBase)
        {
            if (GetSessionValues(ASessionID) != null)
            {
                return true;
            }

            TDBTransaction t = new TDBTransaction();
            bool SubmissionOK = false;
            bool Result = false;

            ADataBase.WriteTransaction(ref t,
                ref SubmissionOK,
                delegate
                {
                    string sql = "SELECT COUNT(*) FROM PUB_s_session WHERE s_session_id_c = ? and s_valid_until_d > NOW()";
                    OdbcParameter[] parameters = new OdbcParameter[1];
                    parameters[0] = new OdbcParameter("s_session_id_c", OdbcType.VarChar);
                    parameters[0].Value = ASessionID;
                    
                    if (Convert.ToInt32(ADataBase.ExecuteScalar(sql, t, parameters)) == 1)
                    {
                        Result = true;
                    }
                    else
                    {
                        // clean all old sessions
                        sql = "DELETE FROM PUB_s_session WHERE s_valid_until_d < NOW()";
                        ADataBase.ExecuteNonQuery(sql, t);
                        SubmissionOK = true;
                    }
                });

            return Result;
        }

        private static SortedList <string, string> GetSessionValuesFromDB(TDataBase ADataBase, out string sessionID)
        {
            bool NewConnection;
            TDataBase db = ConnectDB("GetSessionValuesFromDB", ADataBase, out NewConnection);

            string localSessionID = sessionID = GetSessionID(db);
           
            SortedList <string, string> result = null;
            result = GetSessionValues(sessionID);
            if (result != null)
            {
                if (NewConnection)
                {
                    db.CloseDBConnection();
                }

                return result;
            }

            TDBTransaction t = new TDBTransaction();
            db.ReadTransaction(ref t,
                delegate
                {
                    OdbcParameter[] parameters = new OdbcParameter[1];
                    parameters[0] = new OdbcParameter("s_session_id_c", OdbcType.VarChar);
                    parameters[0].Value = localSessionID;

                    string sql = "SELECT s_session_values_c FROM s_session WHERE s_session_id_c = ?";
                    try
                    {
                        string jsonString = db.ExecuteScalar(sql, t, parameters).ToString();
                        result = JsonConvert.DeserializeObject<SortedList <string, string>>(jsonString);
                        SetSessionValues(localSessionID, result);
                    }
                    catch (Ict.Common.Exceptions.EOPDBException)
                    {
                        result = null;
                    }
                });

            if (NewConnection)
            {
                db.CloseDBConnection();
            }

            return result;
        }

        private static SortedList <string, string> GetSession(string ASessionID, TDataBase ADataBase = null)
        {
            bool NewConnection;
            TDataBase db = ConnectDB("GetSession", ADataBase, out NewConnection);

            string dummy;
            SortedList <string, string> result = GetSessionValuesFromDB(db, out dummy);

            if (result == null)
            {
                TDBTransaction t = new TDBTransaction();
                bool SubmissionOK = false;

                db.WriteTransaction(ref t,
                    ref SubmissionOK,
                    delegate
                    {
                        if (HasValidSession(ASessionID, db))
                        {
                            result = GetSession(ASessionID, db);
                        }
                        else
                        {
                            result = new SortedList <string, string>();
                            SetSessionValues(ASessionID, result);
                        }

                        SubmissionOK = true;
                    });
            }

            if (NewConnection)
            {
                db.CloseDBConnection();
            }

            return result;
        }

        private static void StoreSession(string ASessionID, SortedList <string, string> ASession, TDataBase ADataBase)
        {
            OdbcParameter[] parameters = new OdbcParameter[3];
            parameters[0] = new OdbcParameter("s_session_values_c", OdbcType.Text);
            parameters[0].Value = JsonConvert.SerializeObject(ASession);
            parameters[1] = new OdbcParameter("s_valid_until_d", OdbcType.DateTime);
            parameters[1].Value = DateTime.Now.AddHours(SessionValidHours);
            parameters[2] = new OdbcParameter("s_session_id_c", OdbcType.VarChar);
            parameters[2].Value = ASessionID;

            string sql = "SELECT COUNT(*) FROM PUB_s_session WHERE s_session_id_c = ?";
            OdbcParameter[] parameters2 = new OdbcParameter[1];
            parameters2[0] = new OdbcParameter("s_session_id_c", OdbcType.VarChar);
            parameters2[0].Value = ASessionID;
            
            if (Convert.ToInt32(ADataBase.ExecuteScalar(sql, ADataBase.Transaction, parameters2)) == 1)
            {
                sql = "UPDATE PUB_s_session SET s_session_values_c = ?, s_valid_until_d = ? WHERE s_session_id_c = ?";
            }
            else
            {
                sql = "INSERT INTO PUB_s_session (s_session_values_c, s_valid_until_d, s_session_id_c) VALUES (?,?,?)";
            }

            ADataBase.ExecuteNonQuery(sql, ADataBase.Transaction, parameters);

            SetSessionValues(ASessionID, ASession);
        }

        /// <summary>
        /// gets the current session id, or creates a new session id if it does not exist yet
        /// </summary>
        public static string GetSessionID(TDataBase ADataBase = null)
        {
            string sessionID = FindSessionID();

            bool NewConnection = false;
            TDataBase db = null;

            if (sessionID != string.Empty)
            {
                db = ConnectDB("GetSessionID", ADataBase, out NewConnection);
            }

            if ((sessionID != string.Empty) && !HasValidSession(sessionID, ADataBase))
            {
                TLogging.LogAtLevel(
                    1,
                    "GetSessionID: client is using a session ID that is not valid anymore! (sessionID = " + sessionID +
                    ") => throwing away current session id!");

                // the client is using a session ID that is not valid anymore
                // throw away current session id
                InitThread(string.Empty);

                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Request.Cookies.Remove("OpenPetraSessionID");
                }

                sessionID = string.Empty;
            }

            if (NewConnection)
            {
                db.CloseDBConnection();
            }

            if (sessionID == string.Empty)
            {
                TLogging.LogAtLevel(1, "GetSessionID: sessionID == string.Empty! --> creating new session");
                sessionID = Guid.NewGuid().ToString();

                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Request.Cookies.Add(new HttpCookie("OpenPetraSessionID", sessionID));
                    HttpContext.Current.Response.Cookies.Add(new HttpCookie("OpenPetraSessionID", sessionID));
                }
                else
                {
                    TLogging.LogAtLevel(4, "thread id " + Thread.CurrentThread.ManagedThreadId.ToString());
                    InitThread(sessionID);
                }

                TLogging.LogAtLevel(1, "GetSessionID: new sessionID = " + sessionID);
            }
            else
            {
                // TLogging.LogAtLevel(4, "GetSessionID: sessionID = " + sessionID);
            }

            return sessionID;
        }

        /// <summary>
        /// set a session variable
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="ADataBase"></param>
        public static void SetVariable(string name, object value, TDataBase ADataBase = null)
        {
            bool NewConnection;
            TDataBase db = ConnectDB("SessionSetVariable", ADataBase, out NewConnection);

            TDBTransaction t = new TDBTransaction();
            bool SubmissionOK = false;

            db.WriteTransaction(ref t, ref SubmissionOK,
                delegate
                {
                    string sessionID = GetSessionID(db);
                    SortedList <string, string>session;

                    if (HasValidSession(sessionID, db))
                    {
                        session = GetSession(sessionID, db);
                    }
                    else
                    {
                        session = new SortedList <string, string>();
                    }

                    if (session.Keys.Contains(name))
                    {
                        session[name] = (new TVariant(value)).EncodeToString();
                    }
                    else
                    {
                        session.Add(name, (new TVariant(value)).EncodeToString());
                    }

                    StoreSession(sessionID, session, db);

                    SubmissionOK = true;
                });

            if (NewConnection)
            {
                db.CloseDBConnection();
            }
        }

        /// <summary>
        /// returns true if variable exists and is not null
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ADataBase"></param>
        /// <returns></returns>
        public static bool HasVariable(string name, TDataBase ADataBase = null)
        {
            string sessionID;
            SortedList <string, string>session = GetSessionValuesFromDB(ADataBase, out sessionID);

            bool result = false;

            if ((session != null) && session.Keys.Contains(name) && (session[name] != null))
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// get a session variable
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ADataBase"></param>
        /// <returns></returns>
        public static object GetVariable(string name, TDataBase ADataBase = null)
        {
            string sessionID;
            SortedList <string, string>session = GetSessionValuesFromDB(ADataBase, out sessionID);

            if ((session != null) && session.Keys.Contains(name))
            {
                return TVariant.DecodeFromString(session[name]).ToObject();
            }

            return null;
        }

        /// <summary>
        /// get a session variable, not decoded yet
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ADataBase"></param>
        /// <returns></returns>
        public static TVariant GetVariant(string name, TDataBase ADataBase = null)
        {
            string sessionID;
            SortedList <string, string>session = GetSessionValuesFromDB(ADataBase, out sessionID);

            if ((session != null) && session.Keys.Contains(name))
            {
                return TVariant.DecodeFromString(session[name]);
            }

            return new TVariant((object)null);
        }

        private static void RemoveSession(string ASessionID, TDataBase ADataBase = null)
        {
            bool NewConnection;
            TDataBase db = ConnectDB("RemoveSession", ADataBase, out NewConnection);
            TDBTransaction t = new TDBTransaction();
            bool SubmissionOK = false;

            db.WriteTransaction(ref t, ref SubmissionOK,
                delegate
                {
                    OdbcParameter[] parameters = new OdbcParameter[1];
                    parameters[0] = new OdbcParameter("s_session_id_c", OdbcType.VarChar);
                    parameters[0].Value = ASessionID;

                    string sql = "DELETE FROM  s_session WHERE s_session_id_c = ?";
                    db.ExecuteNonQuery(sql, t, parameters);
                    SubmissionOK = true;
                });

            SetSessionValues(ASessionID, null);
    
            if (NewConnection)
            {
                db.CloseDBConnection();
            }
        }

        /// <summary>
        /// clear the current session
        /// </summary>
        static public void Clear(TDataBase ADataBase = null)
        {
            TLogging.LogAtLevel(1, "TSession.Clear got called");

            string sessionId = GetSessionID();

            TLogging.LogAtLevel(1, "TSession.Clear: sessionID = " + sessionId);

            if (sessionId.Length > 0)
            {
                RemoveSession(sessionId, ADataBase);
                HttpContext.Current.Request.Cookies.Remove("OpenPetraSessionID");
                HttpContext.Current.Response.Cookies.Remove("OpenPetraSessionID");
                ClearSession();
                TLogging.LogAtLevel(1, "TSession.Clear: cleared session with sessionID = " + sessionId);
            }
        }
    }
}
